using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int hp, maxHp, def;
    GameObject downcell;

    private GameManager GameManager;

    void Start()
    {
        hp = 20;
        def = 2;
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //用射线得到自己脚下站的cell，起射点的z坐标设为-2（因为英雄和高台等设备的z坐标为-1，cell的z坐标为10）,射线方向为forward。
        //若为cell则将其父节点设为自己。
        Vector3 rayPoint = new Vector3(transform.position.x, transform.position.y, -2);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayPoint, Vector3.forward);
        foreach (var item in hit)
        {
            if (item.collider.tag == "cell")
            {
                downcell = item.collider.gameObject;
                downcell.GetComponent<Cell>().canpass = false;
                item.collider.GetComponent<Cell>().oncell = this.gameObject;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public GameObject destorybutton;
    bool buttonactive;

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
    public void DestoryWall()
    {
        downcell.GetComponent<Cell>().isblocked = false;
       //删墙后再判断一次敌人路径是否改变
        downcell.GetComponent<Cell>().canpass = true;
        GameManager.GetComponent<PathShower>().IfReach();
        if (GameManager.GetComponent<PathShower>().findway)
        {
            GameManager.GetComponent<PathShower>().FindPath();
        }

        GameObject.Destroy(this.gameObject);
    }



}
