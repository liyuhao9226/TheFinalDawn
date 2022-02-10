using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public GameObject target=null;
   
    int speed = 5;
    //飞行道具向敌人或英雄移动
    private void MoveToTarget()
    {
        if (target != null&&(target.CompareTag ("enemy")|| target.CompareTag("heropiece")))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //敌人不存在时，销毁自身。
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

   public int damage;
    //与目标敌人碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject==target)
        {
            //为敌人时调用伤害函数
            if (target.CompareTag("enemy"))
            {
                damage = damage - target.GetComponent<Enemy>().def;
            if(damage > 0) { target.GetComponent<Enemy>().TakeDamage(damage); }         
            GameObject.Destroy(this.gameObject);
            }
            //为英雄时进行治愈
            if (target.CompareTag("heropiece"))
            {
                target.GetComponent<HeroBasic>().hp += damage;
                //超血量上限时去掉溢出的部分
                if(target.GetComponent<HeroBasic>().hp> target.GetComponent<HeroBasic>().maxHp) { target.GetComponent<HeroBasic>().hp = target.GetComponent<HeroBasic>().maxHp; }
                    GameObject.Destroy(this.gameObject);
            }
        }
    }
}
