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
public class Thron : MonoBehaviour
{
    private GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {
        ReleaseBlock();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //可放置数量减少
        GameManager.setnum[4]--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //地刺在开始战斗后敌人行动前解除其下方cell块的isblocked状态使敌人可以通过
    private void ReleaseBlock()
    {
        Vector2 rayPoint = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayPoint, Vector3.forward);
        foreach (var item in hit)
        {
            if (item.collider.tag == "cell") { item.collider.GetComponent<Cell>().isblocked = false; }
        }
       
    }

    public float attackspeed=1.5f, attackcooldown=3;
    public int atk = 4;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //敌人进入时初始化地刺伤害的数据，并将onthron置true开始计算伤害
        if (other.tag == "enemy")
        {
            InitialThronDamage(other.gameObject);
            other.GetComponent<Enemy>().onthron = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //敌人退出时onthron置false，停止计算伤害。将throndot置为初始值。
        if (other.tag == "enemy")
        {
            other.GetComponent<Enemy>().onthron  = false;
            other.GetComponent<Enemy>().throndot = 3;  
        }
    }

    private void InitialThronDamage(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().thronatk = atk;
        enemy.GetComponent<Enemy>().thronattackspeed = attackspeed;
        enemy.GetComponent<Enemy>().thronattackcooldown = attackcooldown;
    }

    bool buttonactive;
    public GameObject destorybutton;
    private void OnMouseDown()
    {
        if (buttonactive != true)
        {
            destorybutton.SetActive(true);
            buttonactive = true;
        }
        else
        {
            destorybutton.SetActive(false);
            buttonactive = false;
        }

    }



    //撤除
    public void DestoryThron()
    {
        //可放置数量增加
        GameManager.setnum[4]++;
        
        GameObject.Destroy(this.gameObject);
    }


}




//用trigerstay函数有个问题是敌人若在地刺上停止则不会调用这个函数。
/*
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {

            other.GetComponent<Enemy>().throndot += attackspeed * Time.deltaTime;
            if (other.GetComponent<Enemy>().throndot >= attackcooldown)
            {
                other.GetComponent<Enemy>().TakeDamage(4);
                other.GetComponent<Enemy>().throndot = 0;
            }
        }
    }
    */
