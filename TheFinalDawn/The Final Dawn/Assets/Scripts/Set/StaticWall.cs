using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWall : MonoBehaviour
{
    GameObject downcell;

    private GameManager GameManager;

    void Start()
    {
        
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



  

   
  

}
