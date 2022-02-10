using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathShower : MonoBehaviour
{
    //开始时根据所有可通过cell块维护一个矩阵用于寻路
    GameObject[] Cells;
    int[] aa = new int[300];
    void Start()
    {
        OpenList = new List<GameObject>();
        ClosedList = new List<GameObject>();
        TempClosedList = new List<GameObject>();
        //初始化移动速度
        // movespeed = 1 * Time.deltaTime;
        Cells = GameObject.FindGameObjectsWithTag("cell");


        IfReach();
        if (findway) { FindPath(); }
      


    }




    //找到cell对应的序号
    int FindinCells(GameObject cell)
    {

        for (int i = 0; i < 300; i++)
        {
            if (cell == Cells[i]) { return i; }
        }
        return 99;
    }
    //
    void ParentAllocation(GameObject cell, GameObject parent)
    {
        aa[FindinCells(cell)] = FindinCells(parent);
    }
    GameObject FindParent(GameObject cell)
    {
        if (cell != StartNode)
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
            item.GetComponent<Cell>().pathcell.SetActive (false);
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
        //显示移动路径
        foreach (var cell in ClosedList)
        {
            cell.GetComponent<Cell>().pathcell.SetActive(true);
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
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint + Vector2.right, Vector2.right);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint + Vector2.left, Vector2.left);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }
        hit = Physics2D.Raycast(rayPoint + Vector2.down, Vector2.down);
        if (hit.collider != null && hit.collider.CompareTag("cell")) { neighbors.Add(hit.collider.gameObject); }


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
            if (item.collider != null && item.collider.CompareTag("cell")&&cell.GetComponent <Cell>().canpass) { neighbors.Add(item.collider.gameObject); break; }
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








    //洪水算法找无限步数时两点间是否可达（找两点间是否是死路）
    public bool findway=true;
    public void IfReach()
    {
        findway = false;
        List<GameObject> now = new List<GameObject>();
        List<GameObject> open = new List<GameObject>();
        List<GameObject> closed = new List<GameObject>();

        now.Add(StartNode);
        for (int i = 0; i < 250; i++)
        {
            foreach (var current in now)
            {
                closed.Add(current);
                List<GameObject> neighbors = GetNeighbor1(current);
                foreach (var neighbor in neighbors)
                {
                    if (Findin(neighbor, closed)) { continue; }
                    if (!Findin(neighbor, open))
                    {
                        open.Add(neighbor);

                    }
                }

            }
            now.Clear();
            foreach (var item in open)
            {
                now.Add(item);
            }
            open.Clear();
            //找到终点提前跳出循环
            foreach (var item in now)
            {
                if (item == EndNode) { findway = true; break; }
            }

        }
  
    }







    // Update is called once per frame
    void Update()
    {
      


    }









}
