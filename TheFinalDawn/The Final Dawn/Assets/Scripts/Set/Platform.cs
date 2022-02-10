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
public class Platform : MonoBehaviour
{
    public GameObject placecell;
    public bool isblocked = false;
    private GameManager GameManager;


    public GameObject destorybutton;
    bool buttonactive;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        placecell.SetActive(false);
        //可放置数量减少
        GameManager.setnum[3]--;


        //用射线得到自己脚下站的cell，起射点的z坐标设为-2（因为英雄和高台等设备的z坐标为-1，cell的z坐标为10）,射线方向为forward。
        //若为cell则将其父节点设为自己。
        Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y, -2);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayPoint, Vector3.forward);
        foreach (var item in hit)
        {
            if (item.collider.tag == "cell")
            {
                downcell = item.collider.gameObject;
                item.collider.GetComponent<Cell>().oncell = this.gameObject;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (isblocked == false)
        {
            placecell.SetActive(true);
            //鼠标在cell块上时将其传给GameManager
            GameManager.selectplatform = this.gameObject;

        }
    }
    private void OnMouseExit()
    {
        if (GameManager.selectplatform == this.gameObject) { GameManager.GetComponent<GameManager>().selectplatform = null; }
        placecell.SetActive(false);

    }


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
            buttonactive = false ;
        }

    }


    GameObject downcell;
    public GameObject onhero;
    public int hp, maxHp, def;


    public void TakeDamage(int damage)
    {
        if (damage - def > 0)
        {
            hp -= (damage - def);
        }
        if (hp <= 0)
        {
            //若高台上有英雄则把自己脚下的cell与之相连
            if (onhero != null)
            {
                downcell.GetComponent<Cell>().oncell = onhero;
                onhero.GetComponent<HeroBasic>().downcell = downcell;
            }
            else
            {
                downcell.GetComponent<Cell>().isblocked = false;
            }

            GameObject.Destroy(this.gameObject);         
        }
    }
    //撤除
    public void DestoryPlatform()
    {
        //可放置数量增加
        GameManager.setnum[3]++;
        downcell.GetComponent<Cell>().isblocked = false;
        GameObject.Destroy(this.gameObject);
    }

}
