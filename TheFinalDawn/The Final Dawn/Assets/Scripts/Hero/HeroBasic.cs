using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************************
 脚本描述
 每个英雄单位都挂载的基础脚本，包含各种生命，攻击防御等基础属性以
 及攻击范围初始化，受伤判定等基础功能函数。
 
 功能：

 内接外接口：
 
 外接内接口：

 注意事项：

****************************************************************/


public class HeroBasic : MonoBehaviour
{
    public float attackrange;
    public int attackspeed, atk;
    public int maxHp,hp, def;
    public int blocknumber;




    private GameManager GameManager;
    private AttackRangeOpt AttackRangeOpt;
    //指向自己脚下的cell或高台
    public GameObject downcell,downplatform;
    public List<GameObject> BlockedEnemy;

    bool onplatform = false;
    //英雄朝向，初始设为朝右，1234分别表示上下左右
    public int herochaoxiang = 4;

    //用于控制动画
    public Animator ani;

    void Start()
    {
      
 

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ani = GetComponent<Animator>();
        this.transform.GetChild(0).GetComponent<HeroAttack>().ani = ani;
        this.transform.GetChild(0).GetComponent<HeroAttack>().heroposition = transform.position;
        SetNumUpdate1();
        //用射线得到自己脚下站的cell，起射点的z坐标设为-2（因为英雄和高台等设备的z坐标为-1，cell的z坐标为10）,射线方向为forward。
        // Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y, -2);
        //  RaycastHit2D hit = Physics2D.Raycast(rayPoint, Vector3.forward);


        //用射线得到自己脚下站的物体，起射点的z坐标设为-2（因为英雄和高台等设备的z坐标为-1，cell的z坐标为10）,射线方向为forward。
        //若为cell则将其父节点设为自己。
        //判断自己是否在高台上，是则将onplatform置true,并将自己指向其onhero
        Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y, -2);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayPoint, Vector3.forward);
        foreach (var item in hit)
        {
            if (item.collider.tag == "platform")
            {
                onplatform = true;
                item.collider.GetComponent<Platform>().onhero = this.gameObject;
                downplatform = item.collider.gameObject;
                break;
            }
           
        }
        foreach (var item in hit)
        {
            if (item.collider.tag == "cell"&&!onplatform)
            {
                downcell = item.collider.gameObject;
                item.collider.GetComponent<Cell>().oncell = this.gameObject;
            }
        }

            InitialAttackRange();



     

    }

    // Update is called once per frame
    void Update()
    {




         
    }
    //用于显示/关闭操作按钮
    public GameObject destorybutton;
    public bool buttonactive;

   private void OnMouseDown()
    {
        if (buttonactive != true)
        {
            destorybutton.SetActive(true);
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled =true;
            buttonactive = true;

        }
        else
        {
            destorybutton.SetActive(false);
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            buttonactive = false;
        }

    }
    public int tiewei, zhiliao, juji;
    //更新设施数量信息,1为减，2为加
    void SetNumUpdate1()
    {
        GameManager.setnum[0] -= tiewei;
        GameManager.setnum[1] -= zhiliao;
        GameManager.setnum[2] -= juji;
    }
   public void SetNumUpdate2()
    {
        GameManager.setnum[0] += tiewei;
        GameManager.setnum[1] += zhiliao;
        GameManager.setnum[2] += juji;
    }
    //是否为辅助
    public bool fuzhu;
    public GameObject zengyi;
    //撤除
    public void DestoryHero()
    {
        if(onplatform)
        {
            downplatform.GetComponent<Platform>().isblocked = false; 
        }
        else
        {
            downcell.GetComponent<Cell>().isblocked = false;
        }
        SetNumUpdate2();
        //为辅助时撤除时修改范围内英雄数据
        if (fuzhu)
        {
            if (this.transform.GetChild(0).GetComponent<HeroAttack>().heroList.Count > 0)
            {
                foreach (var item in this.transform.GetChild(0).GetComponent<HeroAttack>().heroList)
                {
                    item.GetComponent<HeroBasic>().atk -= 2;
                    item.GetComponent<HeroBasic>().def -= 2;
                    //关闭增益光环
                    item.GetComponent<HeroBasic>().zengyi.SetActive(false);
                }
            }
        }
        GameObject.Destroy(this.gameObject);
    }

    //攻击范围为1时作特殊处理
   // public GameObject range1;
    //用于建造时初始化攻击范围
    void InitialAttackRange()
    {
        //将初始单位攻击范围乘自己的范围系数
        Vector3 tempattackrange = new Vector3(this.transform.GetChild(0).localScale.x * attackrange, this.transform.GetChild(0).localScale.y * attackrange, this.transform.GetChild(0).localScale.z);
        //在高台上则进一步修改攻击范围
        if (onplatform)
        {
            tempattackrange = new Vector3(tempattackrange.x + 2f, tempattackrange.y + 2f, tempattackrange.z);
            attackrange += 2;
        }

        if(attackrange == 1) { tempattackrange = new Vector3(this.transform.GetChild(0).localScale.x * 2, this.transform.GetChild(0).localScale.y , this.transform.GetChild(0).localScale.z); }
        this.transform.GetChild(0).localScale = tempattackrange;

        //生成英雄棋子时找到攻击范围显示的UI并将其展示在与自己位置相同的地方。
        //将isopt置true使AttackRangeOpt开始行使方向选择功能。
        AttackRangeOpt = GameObject.Find("AttackRangeOpt").GetComponent<AttackRangeOpt>();
        AttackRangeOpt.hero = this.gameObject;
        AttackRangeOpt.Range = this.transform.GetChild(0).GetComponent<Transform>();
        AttackRangeOpt.RangeImage = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        AttackRangeOpt.attackrange = attackrange;
        AttackRangeOpt.isopt = true;
        AttackRangeOpt.isfirst = true;

        this.transform.GetChild(0).GetComponent<HeroAttack>().projcetileposition = transform.position;
        this.transform.GetChild(0).GetComponent<HeroAttack>().damage = atk;

    }
    //用于改变攻击范围
    public void AttackRangeChange()
    {
        //将初始单位攻击范围乘自己的范围系数
        Vector3 tempattackrange = new Vector3(attackrange , attackrange, this.transform.GetChild(0).localScale.z);
        this.transform.GetChild(0).localScale = tempattackrange;
        float attackrangepianyi = attackrange / 2 - 0.5f;
        //注意把z轴坐标设为2避免遮盖！！！！！！！！
        switch (herochaoxiang)
        {
            case 1: this.transform.GetChild(0).position = new Vector3(transform.position.x, transform.position.y + attackrangepianyi, 2); break;
            case 2: this.transform.GetChild(0).position = new Vector3(transform.position.x, transform.position.y - attackrangepianyi, 2); break;
            case 3: this.transform.GetChild(0).position = new Vector3(transform.position.x - attackrangepianyi, transform.position.y, 2); break;
            case 4: this.transform.GetChild(0).position = new Vector3(transform.position.x + attackrangepianyi, transform.position.y, 2); break;
        }

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    //用于承受伤害
    public void TakeDamage(int damage)
    {
        if (damage - def > 0)
        {
            hp -= (damage - def);
        }
        if (hp <= 0)
        {
            SetNumUpdate2();
            GameObject.Destroy(this.gameObject);
            downcell.GetComponent<Cell>().isblocked = false;
        }
    }


}
