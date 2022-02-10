using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    public Animator ani;
    private GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        attackingenemy.Clear();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {  
        //不为战斗时清空
        if(!GameManager.battleon) { attackingenemy.Clear(); }
        if(GameManager .battleon)
        {
          //不为治疗时采用攻击函数
        if (!zhiliao) {  Attack(); }
        //为治疗时采用治疗函数
        if (zhiliao) {Cure(); }
        }
     
       
    }
     
    //有敌人进入攻击范围时得到唯一攻击目标
    public List<GameObject> attackingenemy= new List<GameObject>();
    private List<GameObject> enemyList= new List<GameObject>();
    private Queue<GameObject> monsters=new Queue<GameObject>();

    //范围内友军链表
    public List<GameObject> heroList = new List<GameObject>();
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
              
        //加入攻击链表中
        if (other.tag == "enemy" )
        {
            enemyList.Add (other.gameObject); 

        }
        //友军在攻击范围内加入友军队列
        if (other.tag == "heropiece")
        {
            heroList.Add(other.gameObject);
            //自身若是辅助，则给该友军加buff
            if(fuzhu)
            {
                other.GetComponent<HeroBasic>().atk += 2;
                other.GetComponent<HeroBasic>().atk += 2;
                other.GetComponent<HeroBasic>().zengyi.SetActive (true);
            }
        }
      

    }
    //敌人退出时若是攻击目标则取消目标，若在攻击链表中将之删除
    void OnTriggerExit2D(Collider2D other)
    {
       
        //在攻击队列中则去除
        if(InEnemyList (other.gameObject))
        {
            enemyList.Remove(other.gameObject);
        }
        //为当前攻击目标则取消
        RemoveAttackingEnemy(other.gameObject);

        //改变范围时去除不在范围内的友军
        //友军在攻击范围内加入友军队列
        if (InHeroList(other.gameObject))
        {
            heroList.Remove(other.gameObject);
        }
    }
    void RemoveAttackingEnemy(GameObject enem)
    {
        if (attackingenemy != null&&enem.tag =="enemy")
        {
            foreach (var item in attackingenemy)
            {
                //若为正在攻击的敌人，则去除并把所有单位进一
                if (item == enem)
                {
                    attackingenemy.Remove(enem);
                    break;
                }
            }
        }

      
    }
    

    //定义攻击速度与冷却和判断可否攻击的额bool变量
    public int attackspeed=2;
    float attackcooldown=3;
    bool canattack;
    //定义是否是近战
    public bool jinzhan;
    //定义是否是治疗/辅助
    public bool zhiliao,fuzhu;


    //一次可以攻击的敌人数量
    public int onceattacknumber=1;
    //攻击函数，在update函数中调用
    void Attack()
    {
       
        for (int i=0; i<onceattacknumber;i++)
        {
            if (attackingenemy.Count <onceattacknumber && enemyList.Count > 0)
            {
                attackingenemy.Add (enemyList [0]);
                //将自身加入到相应敌人的攻击链表中，用以敌人死亡时去除该敌人
                enemyList[0].GetComponent<Enemy>().attackinghero.Add(this.gameObject);
                enemyList.Remove(enemyList[0]);
            }
        }
     
        //使用攻击队列的敌人数目>0作为条件
        if (attackingenemy.Count>0&&!jinzhan)
        {
            if (canattack == false)
            {
                attackcooldown += attackspeed * Time.deltaTime;
            }
            if(attackcooldown >= 3)
            {
                attackcooldown = 0;
                canattack = true;
            }
           //可以攻击时制造一个飞行道具并将canattack置否
            if (canattack)
            {
                ani.SetBool("isattack", true); 
                CreateProjectile();
                canattack = false;
            }
        }
        //为近战时要额外判断位置信息
        else if(attackingenemy.Count> 0&&jinzhan&& CanJinzhanAtack())
        {
            if (canattack == false)
            {
                attackcooldown += attackspeed * Time.deltaTime;
            }
            if (attackcooldown >= 3)
            {
                attackcooldown = 0;
                canattack = true;
            }
            //可以攻击时制造一个飞行道具并将canattack置否
            if (canattack)
            {
                ani.SetBool("isattack", true);
                CreateProjectile1();
                canattack = false;
            }
        }
       
    }

    //治疗函数
    void Cure()
    {
        //使用英雄队列的英雄数目>0作为条件
        if (heroList.Count > 0)
        {
            if (canattack == false)
            {
                attackcooldown += attackspeed * Time.deltaTime;
            }
            if (attackcooldown >= 3)
            {
                attackcooldown = 0;
                canattack = true;
            }
            //可以攻击时制造一个飞行道具并将canattack置否
            if (canattack)
            {
                ani.SetBool("isattack", true);
                CreateProjectile2();
                canattack = false;
            }
        }
    }

    //判断敌人是否在攻击队列里
    bool InEnemyList(GameObject enem)
    {
        if(enemyList != null)
        {
            foreach (var item in enemyList)
            {
                if (item == enem)
                {
                    return true;
                }
            }
        }
        return false;
    }
    //判断英雄是否在英雄队列里
    bool InHeroList(GameObject enem)
    {
        if (heroList != null)
        {
            foreach (var item in heroList)
            {
                if (item == enem)
                {
                    return true;
                }
            }
        }
        return false;
    }



    //生成飞行道具
    //定义用于临时生成的变量，在外部赋值。
    public GameObject projectile;
    public int damage;
    //生成函数，
    public Vector2 projcetileposition;
    public void CreateProjectile()
    {
       
        foreach(var item in attackingenemy)
        {
            if ( item!= null)
            {
               // projectile.transform.GetComponent<SpriteRenderer>().enabled = true;
                //要先修改target的值，否则生成的飞行道具还是取的上一次的值
                projectile.GetComponent<Projectile>().target = item;
                projectile.GetComponent<Projectile>().damage = damage;
                Instantiate(projectile, projcetileposition, Quaternion.identity);
            }
        }   
    }
    //近战时使用该生成方法
    public void CreateProjectile1()
    {
        //隐藏飞行道具的图片
     //   projectile.transform.GetComponent<SpriteRenderer>().enabled = false; 
        foreach (var item in attackingenemy)
        {        
            //在敌人近身时才攻击
            if (item != null&&Jinshen(item))
            {
                //要先修改target的值，否则生成的飞行道具还是取的上一次的值
                projectile.GetComponent<Projectile>().target = item;
                projectile.GetComponent<Projectile>().damage = damage;
                Instantiate(projectile, projcetileposition, Quaternion.identity);
            }
        }
    }
    //判断是否近身
    public Vector2 heroposition;
    bool Jinshen(GameObject temp)
    {
        if (Mathf.Abs(temp .transform .position .x-heroposition .x)<=1.2&& Mathf.Abs(temp.transform.position.y - heroposition.y) <= 1.2) {return true; }
        return false;
    }
    //判断近战是否可攻击
    bool first=true;
    bool CanJinzhanAtack()
    {
        foreach (var item in attackingenemy)
        {
            //攻击队列中有人近身便可攻击
            if (Jinshen(item)) {   return true; }
        }
        return false;
    }
    //治疗时使用该生成方法
    public void CreateProjectile2()
    {
       
       
        //群体治疗时直接奶所有
        if (onceattacknumber == 99)
        {
            foreach (var item in heroList)
            {
                //要先修改target的值，否则生成的飞行道具还是取的上一次的值
                //先将飞道图片置true避免被辅助隐藏
                projectile.transform.GetComponent<SpriteRenderer>().enabled = true;
                projectile.GetComponent<Projectile>().target = item;
                projectile.GetComponent<Projectile>().damage = damage;
                Instantiate(projectile, projcetileposition, Quaternion.identity);
            }
        }
        //单体治疗
        else
        {
            GameObject cureaim=heroList[0];
      
        //找出血量最低的英雄
        foreach (var item in heroList)
        {
            if (item != null)
            {
                 if(item.GetComponent <HeroBasic >().hp<=cureaim .GetComponent <HeroBasic>().hp) { cureaim = item; }                            
            }
        }
            //要先修改target的值，否则生成的飞行道具还是取的上一次的值
            //是辅助就把治疗量变0且隐藏飞道，否则展示飞道
               if (fuzhu) {projectile.transform.GetComponent<SpriteRenderer>().enabled = false; damage = 0; } else { projectile.transform.GetComponent<SpriteRenderer>().enabled = true; }
                projectile.GetComponent<Projectile>().target = cureaim;
                projectile.GetComponent<Projectile>().damage = damage;
                Instantiate(projectile, projcetileposition, Quaternion.identity);
        }
       
    }

}
