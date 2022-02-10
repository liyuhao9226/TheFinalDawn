using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************************
 脚本描述：
 功能：使用A*寻路算法找到最短路径并将目标逐步移动至目标位置，中途
 遇到阻碍则停止。

 接口：
 1.GameObject类型变量：StartNode和EndNode分别为开始节点和目标节点，
  由外部给出或用代码赋值。
 2.bool类型变量run控制移动动画的演示，将run置false可暂停移动。在外
  部发现run为false时可知棋子遇到阻碍。
 
 注意事项：
 1.进行寻路的路径需要设置GameObject类型的父节点parentNode以供A*算
  法使用。
 2.为实现“遇到阻挡停止”，需要用到Cell设置的isblocked变量。
 3.本脚本针对4联通寻路。但修改GetNeighbor函数的逻辑后可变为8联通。
 ****************************************************************/

public class PathManager : MonoBehaviour
{
    //开始时根据所有可通过cell块维护一个矩阵用于寻路
     GameObject []Cells;
    int[] aa = new int[300];
    private GameObject GameManager;
    //找到核心
    private Core Core;
    public bool start;
    void Start()
    {
        OpenList = new List<GameObject>();
        ClosedList = new List<GameObject>();
        TempClosedList = new List<GameObject>();
        //初始化移动速度
       // movespeed = 1 * Time.deltaTime;
        Cells = GameObject.FindGameObjectsWithTag("cell");

        GameManager = GameObject.Find("GameManager");
        Core = GameObject.Find("Core").GetComponent<Core>();

        EndNode = GameManager.GetComponent<PathShower>().EndNode;
        //根据自身与终点的相对位置判断自己采用哪条路径
        if (transform.position.x - EndNode.transform.position.x < 0 && transform.position.y - EndNode.transform.position.y > 0)
        { StartNode = GameManager.GetComponent<PathShower>().StartNode; }
        else { StartNode = GameManager.GetComponent<PathShowerNorthEast >().StartNode; }
         FindPath();
        run = true;
        start = true;
    }


     

    //找到cell对应的序号
    int FindinCells(GameObject cell)
    {
        
        for(int i=0; i<300;i++)
        {
            if(cell== Cells[i]) { return i; }
        }
        return 99;
    }
    //
    void ParentAllocation (GameObject cell,GameObject parent)
    {
        aa[FindinCells(cell)] = FindinCells(parent);
    }
    GameObject  FindParent(GameObject cell)
    {
        if (cell!=StartNode)
        {
            return Cells[aa[FindinCells(cell)]].gameObject;
        }
        else return null;
    }


    private List<GameObject> OpenList;
    public List<GameObject> ClosedList;

    //A*寻路的起点与终点,在其他脚本中赋值
    public GameObject StartNode;
    public GameObject EndNode;
   

    //清空寻路算法用到的所有链表，并将其中所有cell的parent置空
    public void ClearNode()
    {
        foreach (var item in ClosedList)
        {
            item.GetComponent<Cell>().parentNode = null;
        }
        foreach (var item in OpenList)
        {
            item.GetComponent<Cell>().parentNode = null;
        }
        OpenList.Clear();
        ClosedList.Clear();
        TempClosedList.Clear();

    }
    //A*寻路第一部分，以fcost为指标填充ClosedList并确定cell间的亲子关系。
    public void FindPath()
    {
     
        if (true)
        {
            ClearNode();
            OpenList.Add(StartNode);
            for (int i = 0; i < 999; i++)
            {
               
                GameObject current = LowestFCost(OpenList);
                OpenList.Remove(current);
                ClosedList.Add(current);
                if (current == EndNode)
                {
                    break;
                }
                List<GameObject> neighbors = GetNeighbor1(current);
                foreach (var cell in neighbors)
                {
                    if (Findin(cell, ClosedList)) { continue; }
                    if (!Findin(cell, OpenList))
                    {
                       // cell.GetComponent<Cell>().CalculateCost();

                       // cell.GetComponent<Cell>().parentNode = current;

                        ParentAllocation(cell, current);
                        OpenList.Add(cell);

                    }
                }

            }
            FindShort1();
        }
        //找到路径后，run置true，开始移动。
       // foreach(var cell in ClosedList) { Debug.Log(cell.transform .position .x); }
       // if (ClosedList != null) { run = true; }
    }



    //用于逆向寻找最短路径并倒置
    private List<GameObject> TempClosedList;
    private GameObject TheParent;
    //A*寻路第二部分，从终点以parent为导向回溯找到最短路径，并将之倒置放于ClosedList中
    private void FindShort()
    {
        TempClosedList.Add(EndNode);
        TheParent = EndNode.GetComponent<Cell>().parentNode;
        while (TheParent != null)
        {
            TempClosedList.Add(TheParent);
            TheParent = TheParent.GetComponent<Cell>().parentNode;
        }
        int i = -1;
        foreach (var item in TempClosedList)
        {
            i++;
        }
        ClosedList.Clear();
        while (i != -1)
        {
            ClosedList.Add(TempClosedList[i]);
            i--;
        }
    }

    private void FindShort1()
    {
        TempClosedList.Add(EndNode);
        TheParent = FindParent(EndNode);
        while (TheParent != null)
        {
            TempClosedList.Add(TheParent);
            TheParent = FindParent(TheParent);
        }
        int i = -1;
        foreach (var item in TempClosedList)
        {
            i++;
        }
        ClosedList.Clear();
        while (i != -1)
        {
            ClosedList.Add(TempClosedList[i]);
            i--;
        }
    }


    //A*寻路算法得到相邻节点
    private List<GameObject> GetNeighbor(GameObject cell)
    {

        List<GameObject> neighbors = new List<GameObject>();
        Vector2 rayPoint = new Vector2(cell.transform.position.x, cell.transform.position.y);
        RaycastHit2D hit;
        //起射点需要加一个偏移量，否则会返回自身（因为是从中心射，会先射到自己）
        hit = Physics2D.Raycast(rayPoint + Vector2.up, Vector2.up);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject);  }
        hit = Physics2D.Raycast(rayPoint + Vector2.right, Vector2.right);
        if (hit.collider != null && hit.collider.CompareTag("cell") ) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint + Vector2.left, Vector2.left);
        if (hit.collider != null && hit.collider.CompareTag("cell") ) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell") ) { neighbors.Add(hit.collider.gameObject); }


        //把该部分代码加上即得8联通情况下的A*寻路算法
        /*
        
        Vector2 rayPoint1 = new Vector2(cell.transform.position.x+1, cell.transform.position.y);
        Vector2 rayPoint2 = new Vector2(cell.transform.position.x-1, cell.transform.position.y);

        hit = Physics2D.Raycast(rayPoint1 + Vector2.up, Vector2.up);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint1 + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint2 + Vector2.up, Vector2.up);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint2 + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        
        */


        return neighbors;
    }


    //该函数是“得到相邻节点”更新后的算法，可以解决cell块因在z轴方向上被遮挡而无法正确被选中的问题
    private List<GameObject> GetNeighbor1(GameObject cell)
    {

        List<GameObject> neighbors = new List<GameObject>();
        Vector2 cellp = new Vector2(cell.transform.position.x, cell.transform.position.y);

        //起射点需要加一个偏移量，否则会返回自身（因为是从中心射，会先射到自己）
        //射线采用raycastall可以返回遮挡物与cell块，用tag==cell的判断条件筛掉遮挡物，用“第一个cell块更近”的情况使用break筛掉后续的cell块。
        //注意射线距离跟遮挡物的数量有关，建议比可能的遮挡物多几个。
        RaycastHit2D[] temp1 = Physics2D.RaycastAll(cellp + Vector2.up, Vector2.up, 3);
        foreach (var item in temp1)
        {
            if (item.collider != null && item.collider.CompareTag("cell") && cell.GetComponent<Cell>().canpass) { neighbors.Add(item.collider.gameObject); break; }
        }
        RaycastHit2D[] temp2 = Physics2D.RaycastAll(cellp + Vector2.down, Vector2.down, 3);
        foreach (var item in temp2)
        {
            if (item.collider != null && item.collider.CompareTag("cell") && cell.GetComponent<Cell>().canpass) { neighbors.Add(item.collider.gameObject); break; }
        }
        RaycastHit2D[] temp3 = Physics2D.RaycastAll(cellp + Vector2.left, Vector2.left, 3);
        foreach (var item in temp3)
        {
            if (item.collider != null && item.collider.CompareTag("cell") && cell.GetComponent<Cell>().canpass) { neighbors.Add(item.collider.gameObject); break; }
        }
        RaycastHit2D[] temp4 = Physics2D.RaycastAll(cellp + Vector2.right, Vector2.right, 3);
        foreach (var item in temp4)
        {
            if (item.collider != null && item.collider.CompareTag("cell") && cell.GetComponent<Cell>().canpass) { neighbors.Add(item.collider.gameObject); break; }
        }

        //把该部分代码加上即得8联通情况下的A*寻路算法
        /*
        
        Vector2 rayPoint1 = new Vector2(cell.transform.position.x+1, cell.transform.position.y);
        Vector2 rayPoint2 = new Vector2(cell.transform.position.x-1, cell.transform.position.y);

        hit = Physics2D.Raycast(rayPoint1 + Vector2.up, Vector2.up);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint1 + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint2 + Vector2.up, Vector2.up);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint2 + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        
        */


        return neighbors;
    }

    //找cell是否在closed中
    private bool Findin(GameObject gb, List<GameObject> _cells)
    {
        foreach (var cell in _cells)
        {
            if (gb == cell)
            { return true; }
        }
        return false;
    }

    //将OpenList中的cell根据fcost的值排列
    private GameObject LowestFCost(List<GameObject> _cells)
    {
        //f用于将开始结点放入
        int f = 1000;
        GameObject output = _cells[0];
        foreach (var cell in _cells)
        {
            //计算cell的fcost
            int fcost = CalculateCost(cell);
            if (fcost < f)
            {
                f = fcost;
                output = cell;
            }
        }
        return output;
    } 
    //A*算法寻路算各个权重
    private int CalculateCost(GameObject cell)
    {
        int scost, ecost, fcost;
        scost = (int)(Mathf.Abs(cell.transform.position.x - StartNode.transform.position.x) + Mathf.Abs(cell.transform.position.y - StartNode.transform.position.y));
        ecost = (int)(Mathf.Abs(cell.transform.position.x - EndNode.transform.position.x) + Mathf.Abs(cell.transform.position.y - EndNode.transform.position.y));
        fcost = scost + ecost;
        return fcost;
        // Debug.Log(fcost +" "+ scost+" "+ecost+" " + PathManager.StartNode + " "+ PathManager.EndNode+" "+ gameObject);
    }






    public int coredamage;
    void Update()
    {
        
        //棋子移动到终点后终止,修改各变量
        if (run == true && transform.position.x == EndNode.transform.position.x && transform.position.y == EndNode.transform.position.y)
        {
            run = false;  i = 0;
            //核心血量减少,敌人数减少
            Core.hp-=coredamage;GameManager.GetComponent<GameManager>().enemynum--;
            GameObject.Destroy(this.gameObject); 
        }
        //显示棋子移动动画
        if (run&&ClosedList[i] != null)
        {
            //移动前判断是否被阻挡,判断后run依然为true则继续走
         
            WhenBlocked();
           

            if (run) { ShowMoveAnimation(ClosedList[i]); }
           
        }

        //没有遮挡后继续前进
      if(isblocked && ClosedList[i].GetComponent <Cell>().isblocked == false && ClosedList[tempi].GetComponent<Cell>().isblocked == false)
        {
            this.GetComponent<Enemy>().attackaim = null;
            isblocked = false;
            run = true;
        }

    }
//移动动画展示部分
    
    //用于展示棋子移动动画的变量。
    //run用于控制棋子移动动画。first用于第一次读取下一块单元格（可理解为：first为true表明棋子正好在一个单元格上；为false时表明棋子在多个单元格上）
    public bool run = false; private bool first = true;
   public int i = 0 , tempi = -1;
    //定义用于展示移动动画的临时变量与棋子移动速度
   public float movespeed;
    //临时存储cell的直角坐标信息用于移动。
    Vector2 nextcell;
    //turn为true表示棋子向右，否则向左
    private bool turn = true;

    bool isblocked;
    //单次显示棋子移动动画
    private void ShowMoveAnimation(GameObject cell)
    {
        //进下一格前得到其坐标信息并判断是否需要转向
        if (first)
        {
            nextcell =new Vector2 (cell.transform.position.x, cell.transform.position.y);
            if(nextcell.x>transform.position.x) { if (!turn) { transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1)); turn = !turn; } }
            if(nextcell.x<transform.position.x) { if (turn) { transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1)); turn = !turn; } }

        }
       
        //朝目标移动
        transform.position = Vector2.MoveTowards(transform.position, nextcell, movespeed*Time.deltaTime);

        //移动后判断是否完全到了下一格，是则取路径上下一个cell
        if (transform.position.x== cell.transform.position.x&& transform.position.y == cell.transform.position.y)
        {
            i++;
            tempi++;
            first = true;  
            //第一个判断用于取消到达终点后的进一步计算（不这样会超出范围报错）
            if (!(transform .position .x == EndNode.transform.position.x && transform.position .y == EndNode.transform.position.y))
            {
                WhenBlocked();
            }
        } 

    }

    


   //移动前判断当前和下一格上是否有阻挡，有则停止移动。
   void WhenBlocked()
    {
        //判断当前格
        if (tempi >= 0)
        {
            if (ClosedList[tempi].GetComponent<Cell>().isblocked == true && this.GetComponent<Enemy>().attackaim == null)
            {
                //阻挡物为英雄则根据其阻挡数判断自己是否被阻挡
                if (ClosedList[tempi].GetComponent<Cell>().oncell.tag == "heropiece")
                {
                    //  Debug.Log(11111);
                    //目标英雄的阻挡数小于其最大阻挡数时将自身加入其阻挡队列中并停止。
                    if (ClosedList[tempi].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().BlockedEnemy.Count < ClosedList[tempi].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().blocknumber)
                    {
                      //  Debug.Log(22222);
                        this.GetComponent<Enemy>().attackaim = ClosedList[tempi].GetComponent<Cell>().oncell;
                        ClosedList[tempi].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().BlockedEnemy.Add(this.gameObject);
                        run = false;
                        isblocked = true;
                    }
                }
            }
        }

        //判断下一格
        if (ClosedList[i].GetComponent<Cell>().isblocked == true&& this.GetComponent<Enemy>().attackaim==null)
        {
            //阻挡物为英雄则根据其阻挡数判断自己是否被阻挡
            if (ClosedList[i].GetComponent<Cell>().oncell.tag == "heropiece")
            {   //目标英雄的阻挡数小于其最大阻挡数时将自身加入其阻挡队列中并停止。
                if (ClosedList[i].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().BlockedEnemy.Count < ClosedList[i].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().blocknumber)
                {
                    this.GetComponent<Enemy>().attackaim  = ClosedList[i].GetComponent<Cell>().oncell;
                    ClosedList[i].GetComponent<Cell>().oncell.GetComponent<HeroBasic>().BlockedEnemy.Add(this.gameObject);
                    run = false;
                    isblocked = true;
                }
            }
            //高台的阻挡能力为无穷，所以非英雄时直接阻挡
            else
            {
                this.GetComponent<Enemy>().attackaim = ClosedList[i].GetComponent<Cell>().oncell;
                run = false;
                isblocked = true;
            }

        }
    }
   
  


}
