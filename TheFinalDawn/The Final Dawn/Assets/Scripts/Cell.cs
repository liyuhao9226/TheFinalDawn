using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************************
 脚本描述
 功能：

 接口：
 
 注意事项：
 1.cell需添加“cell”tag，order in layer 设置为0.
 2.cell初始z坐标为10，其他设施初始z坐标为-1（可在Drag脚本中的GreatSet中修改）
****************************************************************/

public class Cell : MonoBehaviour
{
    public GameObject parentNode = null;
    //指向在cell上的设施
    public GameObject oncell;
    public bool isblocked=false;
    private GameManager GameManager;
    private AttackRangeOpt AttackRangeOpt;
    public GameObject arrowindex;


    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        AttackRangeOpt = GameObject.Find("AttackRangeOpt").GetComponent<AttackRangeOpt>();
    }

    // Update is called once per frame
    void Update()
    {
       if(arrowindex != null)
        {
            if (GameManager.battleon) { arrowindex.SetActive(false); }
            else arrowindex.SetActive(true);
        }
    }


    //用于显示将要放置的cell块
   public GameObject placecell;
   public GameObject pathcell;
    //用于显示攻击范围的cell块

   public bool canpass = true;

    private void OnMouseEnter()
    {
        if (isblocked==false&& AttackRangeOpt.isopt ==false)
        {
            placecell.SetActive(true);
            //鼠标在cell块上时将其传给GameManager的selectcell
            GameManager.selectcell = this.gameObject;
          
        }
        //鼠标在cell块上时将其传给GameManager的selectattackcell
        GameManager.selectattackcell = this.gameObject;
        //若此时正在放墙，将其可通过状态置false
        if(GameManager .wallsetting&& oncell==null)
        {           
            canpass = false;
            PathUpdate();
            /*
            GameManager.GetComponent<PathShower>().IfReach();
            if (GameManager.GetComponent<PathShower>().findway)
            {
                GameManager.GetComponent<PathShower>().FindPath();
            }
            else
            {
                Debug.Log(1);
            }
            */

        }
       

    }
    private void OnMouseExit()
    {
        if (GameManager.selectcell == this.gameObject) { GameManager.GetComponent<GameManager>().selectcell = null; }
        if (GameManager.selectattackcell == this.gameObject) { GameManager.GetComponent<GameManager>().selectattackcell = null; }
        placecell.SetActive(false);
        //若此时正在放墙，将其可通过状态置true
        if (GameManager.wallsetting&& canpass == false && oncell == null)
        {
            canpass = true;
          //  PathUpdate();
        }

    }
       //更新所有路径显示
    void PathUpdate()
    {
        //在该cell格之前在路径上时才重新寻路
        if (pathcell == true)
        {
        GameManager.GetComponent<PathShower>().IfReach();
        if (GameManager.GetComponent<PathShower>().findway)
        {
            GameManager.GetComponent<PathShower>().FindPath();
        }

        GameManager.GetComponent<PathShowerNorthEast>().IfReach();
        if (GameManager.GetComponent<PathShowerNorthEast>().findway)
        {
            GameManager.GetComponent<PathShowerNorthEast>().FindPath();
        }
        }
      
        
    }
    

}
