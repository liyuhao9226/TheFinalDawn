using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************************
 脚本描述
 
 功能：

 内接外接口：
 
 外接内接口：

 注意事项：

****************************************************************/
public class AttackRangeOpt : MonoBehaviour
{    
    //attackrange在各英雄中赋值，isopt在各英雄生成时置true。isstore用于使英雄复制攻击范围，为true时表示可以存储攻击范围了。
    public float attackrange;
    public bool isopt,isfirst;
    
    public GameObject hero;
    //用于隐藏攻击范围显示的图片
    public SpriteRenderer RangeImage;
    //用于旋转攻击范围
    public Transform Range;

   
    //用于开始时放在相机（屏幕）外
    Vector2 temp = new Vector2(100,100);
    void Start()
    {
        
        transform.position = Camera.main.WorldToScreenPoint(temp);
        
    }
   
    
    private Vector2 temporigin;
    void Update()
    {
        if (isopt)
        {   
            transform.position = Input.mousePosition;
            //“展开”范围选择时记下起始坐标，用于判断最终朝向
            if (isfirst) { temporigin = Camera.main.WorldToScreenPoint(hero.transform .position); isfirst = false; }         
            if (ifturn != JudgeDirection()) { TurnRange(JudgeDirection()); }
         
        }
    }




    //按下鼠标，根据UI和棋子的相对位置判断其攻击范围,在各英雄的脚本中把attacklist复制。注意UI的鼠标点击要通过trigger触发。
    public void OnMouseDown()
    {
        isopt = false;
        //放回屏幕外,并隐藏攻击范围的图片
        transform.position = Camera.main.WorldToScreenPoint(temp); 

        //用射线得到自己脚下站的cell，起射点的z坐标设为-2（因为英雄和高台等设备的z坐标为-1，cell的z坐标为10）,射线方向为forward。
         //   Vector3 rayPoint = new Vector3(hero.transform.position.x, hero.transform.position.y, 0);
           // RaycastHit2D hit = Physics2D.Raycast(rayPoint+ Vector3.forward, Vector3.forward);
    
        //取消时消除设施。
        if (xx == 5)
        {
            //取消cell占用
            //若脚下的为cell则将其isblocked置false。否则必为高台，直接置false
            /*
            if (hit.collider.CompareTag("cell")) { hit.collider.GetComponent<Cell>().isblocked = false; hit.collider.GetComponent<Cell>().oncell = null; }
            else
            {
                if(hit.collider.tag =="platform")
                hit.collider.GetComponent<Platform>().isblocked = false;
            }
            */
            if (hero.GetComponent <HeroBasic>().downplatform !=null)
            {
                hero.GetComponent<HeroBasic>().downplatform.GetComponent<Platform>().isblocked = false;
            }
            else if (hero.GetComponent<HeroBasic>().downcell != null)
            {
                hero.GetComponent<HeroBasic>().downcell.GetComponent<Cell>().isblocked = false; hero.GetComponent<HeroBasic>().downcell.GetComponent<Cell>().oncell = null;
            }
            hero.GetComponent<HeroBasic>().SetNumUpdate2();
            GameObject.Destroy(hero.gameObject);           
            hero = null;         
        }
        //确认后隐藏攻击范围的图片
        else
        {
            RangeImage.enabled = false;
           // if (attackrange==1) { hero.GetComponent<HeroBasic>().range1.transform.GetComponent<SpriteRenderer>().enabled = false; }
        }
    }

    //xx用于取消放置（不设全局中间变量而只调用函数或设置局部变量则无法成功取消，原因不明）
     int xx;
    //判断方向，返回1234依次代表上下左右
    public int JudgeDirection()
    {
        if (Mathf.Abs(transform.position.x - temporigin.x)<=10&& Mathf.Abs(transform.position.y - temporigin.y) <= 10) { xx = 5; return 5; }
        else { cancel.SetActive(false); xx = 1; }
        if (transform.position.y - temporigin.y >= Mathf.Abs(transform.position.x - temporigin.x)) { return 1; }
        if (transform.position.y - temporigin.y < 0 && temporigin.y- transform.position.y> Mathf.Abs(transform.position.x - temporigin.x)) { return 2; }
        if (transform.position.x - temporigin.x < 0&& temporigin.x - transform.position.x> Mathf.Abs(transform.position.y - temporigin.y)) { return 3; }
        if (transform.position.x - temporigin.x>= Mathf.Abs(transform.position.y - temporigin.y)) { return 4; }


        return 0;
    }
    //调转方向，1234依次表示上下左右
    //设置ifturn判断是否转向，减少运算量
    int ifturn;
    public GameObject cancel;
 

    void TurnRange(int x)
    {
        ifturn = x;
        //攻击范围的偏移量，用于定位攻击范围。偏移量小于1时说明攻击范围为1，则强制设为0.5
        float attackrangepianyi=attackrange/2-0.5f;
        if(attackrangepianyi < 1) { attackrangepianyi = 0.5f; }
        switch (x)
        {
            case 1: RangeImage.enabled = true; if (attackrange == 1) { Range.localScale = new Vector3(1, 2, Range.localScale.z); }
                Range.position = new Vector3(hero.transform.position.x, hero.transform.position.y + attackrangepianyi, hero.transform.position.z);
                hero.GetComponent<HeroBasic>().herochaoxiang = 1; break;
            case 2: RangeImage.enabled = true; if (attackrange == 1) { Range.localScale = new Vector3(1, 2, Range.localScale.z); }
                Range.position = new Vector3(hero.transform.position.x, hero.transform.position.y - attackrangepianyi, hero.transform.position.z);
                hero.GetComponent<HeroBasic>().herochaoxiang = 2; break;
            case 3: RangeImage.enabled = true; if (attackrange == 1) { Range.localScale = new Vector3(2, 1, Range.localScale.z); }
                Range.position = new Vector3(hero.transform.position.x - attackrangepianyi, hero.transform.position.y, hero.transform.position.z);
                //与当前朝向不同则转向
                if (hero .GetComponent <HeroBasic >().herochaoxiang !=3)
                {
                    hero.transform.GetComponent<SpriteRenderer>().flipX = true;
                   // hero.transform .localScale = Vector3.Scale(hero.transform.localScale, new Vector3(-1, 1, 1));
                }
                hero.GetComponent<HeroBasic>().herochaoxiang = 3;
                break;
            case 4: RangeImage.enabled = true; if (attackrange == 1) { Range.localScale = new Vector3(2, 1, Range.localScale.z); }
                Range.position = new Vector3(hero.transform.position.x + attackrangepianyi, hero.transform.position.y, hero.transform.position.z);
                if (hero.GetComponent<HeroBasic>().herochaoxiang != 4)
                {
                    hero.transform.GetComponent<SpriteRenderer>().flipX = false;
                    //hero.transform.localScale = Vector3.Scale(hero.transform.localScale, new Vector3(-1, 1, 1));
                }
                hero.GetComponent<HeroBasic>().herochaoxiang = 4;
                break;
            case 5: cancel.SetActive(true);RangeImage.enabled=false; break;
        }
        //最后将范围的z轴固定在2！！！！！！！！避免遮盖其他物体！！！！！！！！！！！！
        Range.position = new Vector3(Range.position.x , Range.position.y, 2);

    }










    //一开始的攻击范围选择的逻辑。用射线将攻击范围内的cell放进attackList中再操作。
    /*

    //根据判断的方向填充链表并展示
     public List<GameObject> attackList;
    void GetAttackRange()
    {
        //开始时清空一下attacklist
        CloseAttackRange();
        //将ifturn改为当前方向
        ifturn = JudgeDirection();

        switch (JudgeDirection())
        {

            case 1: UpCell(heroposition); break;
            case 2: DownCell(heroposition); break;
            case 3: LeftCell(heroposition); break;
            case 4: RightCell(heroposition); break;
            case 5: CloseAttackRange(); cancel.SetActive(true); break;
        }
        ShowAttackRange();

    }
    //四个方向获取攻击范围的小函数组
    void UpCell(Vector2 cellp)
    {
        RaycastHit2D[] temp = Physics2D.RaycastAll(cellp + Vector2.up, Vector2.up, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.x++;
        temp = Physics2D.RaycastAll(cellp + Vector2.up, Vector2.up, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.x -= 2;
        temp = Physics2D.RaycastAll(cellp + Vector2.up, Vector2.up, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
    }
    void DownCell(Vector2 cellp)
    {
        RaycastHit2D[] temp = Physics2D.RaycastAll(cellp + Vector2.down, Vector2.down, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.x++;
        temp = Physics2D.RaycastAll(cellp + Vector2.down, Vector2.down, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.x -= 2;
        temp = Physics2D.RaycastAll(cellp + Vector2.down, Vector2.down, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
    }
    void LeftCell(Vector2 cellp)
    {
        RaycastHit2D[] temp = Physics2D.RaycastAll(cellp + Vector2.left, Vector2.left, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.y++;
        temp = Physics2D.RaycastAll(cellp + Vector2.left, Vector2.left, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.y -= 2;
        temp = Physics2D.RaycastAll(cellp + Vector2.left, Vector2.left, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
    }
    void RightCell(Vector2 cellp)
    {
        RaycastHit2D[] temp = Physics2D.RaycastAll(cellp + Vector2.right, Vector2.right, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.y++;
        temp = Physics2D.RaycastAll(cellp + Vector2.right, Vector2.right, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
        cellp.y -= 2;
        temp = Physics2D.RaycastAll(cellp + Vector2.right, Vector2.right, attackrange);
        foreach (var cell in temp)
        {
            if (cell.collider.tag == "cell")
            {
                attackList.Add(cell.collider.gameObject);
            }
        }
    }
    //开关攻击范围显示（关闭时清空链表）。
    void ShowAttackRange()
    {
        if (attackList != null)
        {
            foreach (var cell in attackList)
            {
                cell.GetComponent<Cell>().attackcell.SetActive(true);
            }
        }

    }
    void CloseAttackRange()
    {
        if (attackList != null)
        {
            foreach (var cell in attackList)
            {
                cell.GetComponent<Cell>().attackcell.SetActive(false);
            }
        }
        attackList.Clear();
    }

*/




}
