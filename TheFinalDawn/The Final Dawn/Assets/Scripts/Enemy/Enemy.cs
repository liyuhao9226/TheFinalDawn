using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************************
 脚本描述
 功能：

 接口：
 
 注意事项：

****************************************************************/


public class Enemy : MonoBehaviour
{
   public int maxHp, hp, def, atk;
    public Animator ani;
    private GameManager GameManager;
    //找到核心
    private Core Core;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Core = GameObject.Find("Core").GetComponent<Core>();
 
        ani = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        

        if (this.GetComponent<PathManager>().start&&GameManager .battleon )
        {
            ThronDamage();
            Attack();
        }
    }
    
    //停止/开始移动
   public void PauseRun()
    {
        //PathManager.run = false;
        this.GetComponent<PathManager>().run =!this.GetComponent<PathManager>().run; 
    }

    public void StartRun()
    {
        //PathManager.run = false;
        this.GetComponent<PathManager>().FindPath();
    }
    //地刺伤害
    public float throndot = 3, thronattackspeed, thronattackcooldown;
    public int thronatk,thronresist;
    public bool onthron;


    void ThronDamage()
    {
        if (onthron)
        {
            throndot += thronattackspeed * Time.deltaTime;
            if (throndot >= thronattackcooldown)
            {
                TakeDamage(thronatk-thronresist);
                throndot = 0;
            }
        }

    }

    public void TakeDamage(int damage)
    {
        if(damage - def>0)
        {
            hp -= (damage - def);
        }
        if (hp <= 0)
        {
            EnemyDestroy();
           // GameObject.Destroy(this.gameObject);
        }
    }


    //用于存储“正在攻击这个敌人”的英雄
    public List<GameObject> attackinghero=new List<GameObject>();
    void EnemyDestroy()
    {
        //将自身从相应的阻挡链表中删除
        if (attackaim != null&&attackaim .tag=="heropiece")
        {
            attackaim.GetComponent <HeroBasic>().BlockedEnemy.Remove(this.gameObject);
        }
        //将自身从相应的攻击链表中删除
        if(attackinghero .Count > 0)
        {
            foreach (var item in attackinghero)
        {
            //GameObject.Destroy(this.gameObject);
            if(item!=null)
            item.GetComponent<HeroAttack>().attackingenemy.Remove(this.gameObject);
        }
        }
  
        GameManager.enemynum--;
        GameObject.Destroy(this.gameObject);
    }

    float attackspeed = 1, attackcooldown = 3;
    //用于存储“正在阻挡这个敌人”的英雄
   public GameObject attackaim=null;
    void Attack()
    {
        //run为false时说明遇到障碍，进行攻击动作
        if (!this.GetComponent<PathManager>().run)
        {
       
            if (attackaim != null)
            { 
            attackcooldown += attackspeed * Time.deltaTime;
            if (attackcooldown >= 3)
            {
                attackcooldown = 0;
                ani.SetBool("isattack", true);
                switch (attackaim.tag)
                        {
                            case "heropiece": attackaim.GetComponent<HeroBasic>().TakeDamage(atk); break;
                            case "platform": attackaim.GetComponent<Platform>().TakeDamage(atk); break;
                        }
                    
                   
            }
            }
        }
        
        
    }

}
