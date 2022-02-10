using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************************
 脚本描述
 
 功能：
 前半部分是一个简单的拖拽功能（可直接复制到需求类似功能的脚本中）。
 后半部分为使玩家放置设施set的功能（英雄，地刺，高台等统称为设施）
 玩家拖拽图标到合法cell块或高台上，松开鼠标后即可放置相应设施

 内接外接口：
 1.GameManager脚本。在开始时初始化，用于判断cell块或高台是否能放置
 设施。

 外接内接口：

 注意事项：

****************************************************************/

public class Drag : MonoBehaviour
{
    //表明该按钮用于放置何种设施。12345依次表示铁卫，治疗，狙击，高台，陷阱
    public int kindindex;



    //用于放置失败时将UI放回原位。
    Vector2 originPosition;
    private GameManager GameManager;
    void Start()
    {
        originPosition = transform.position;
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
     
    }

    // Update is called once per frame
    void Update()
    {
        JudgeIfDrag();
        if (ispoint)
        {
            transform.position = Input.mousePosition;
            if(this.tag == "wall")
            {
               // GameManager.GetComponent<PathShower>().IfReach();
                if (GameManager .GetComponent <PathShower>().findway==false || GameManager.GetComponent<PathShowerNorthEast>().findway == false)
                {
                    this.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    this.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

        }


    }
    //|| GameManager.GetComponent<PathShowerNorthEast>().findway == false
    bool candrag = true;
    //灰色图片表示数量达上限
    public GameObject greyimage;
    //判断数量是否达上限和自身能否被拖动
    void JudgeIfDrag()
    {
        switch (kindindex)
        {
            case 1: if (GameManager.setnum[0] <= 0) { candrag = false; greyimage.SetActive(true); } else { candrag = true; greyimage.SetActive(false); } break;
            case 2: if (GameManager.setnum[1] <= 0) { candrag = false; greyimage.SetActive(true); } else { candrag = true; greyimage.SetActive(false); } break;
            case 3: if (GameManager.setnum[2] <= 0) { candrag = false; greyimage.SetActive(true); } else { candrag = true; greyimage.SetActive(false); } break;
            case 4: if (GameManager.setnum[3] <= 0) { candrag = false; greyimage.SetActive(true); } else { candrag = true; greyimage.SetActive(false); } break;
            case 5: if (GameManager.setnum[4] <= 0) { candrag = false; greyimage.SetActive(true); } else { candrag = true; greyimage.SetActive(false); } break;
            case 0: candrag = true; greyimage.SetActive(false); break;
        }
        //在战斗进行时所有放置按钮置灰且不可拖拽
        if(GameManager .battleon) { candrag = false; greyimage.SetActive(true); }

    }

    bool ispoint;
    //按住鼠标时调用
    public void StartDrag()
    {
        if(candrag)
        {
            ispoint = true; info.SetActive(false);
        //若将要放置的是墙，将放置墙的信号量置true
        if (this.tag == "wall")
        {
            GameManager.wallsetting = true;
        }

        }
       
    }
    //抬起鼠标时调用
    public void StopDrag()
    {
        if(candrag)
        {
            ispoint = false;
        //在有路时尝试放置设施。无路时不放置且将当前cell的canpass设为true。
        if (this.tag == "wall")
        {
            if (GameManager.GetComponent<PathShower>().findway &&GameManager.GetComponent<PathShowerNorthEast>().findway)
            {
                CreateSet();
            }
            else
            {
                if (GameManager.selectcell!=null&&GameManager.selectcell.GetComponent<Cell>().oncell == null)
                {
                    GameManager.selectcell.GetComponent<Cell>().canpass = true;
                    this.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else CreateSet(); 
        //使UI回到原位
        transform.position = originPosition;
        //抬起时将放置墙的信号量置false
        GameManager.wallsetting = false;
     
        }
        
    }

    /*
     展示信息相关
         */
    public GameObject info;
    public void ShowInfo()
    {
        if (!ispoint)
        { info.SetActive(true); }
    }
    public void CloseInfo()
    {
        info.SetActive(false);
    }



    /*
     生成设施相关
         */



    //生成装置
    //定义用于临时生成的变量，在外部赋值。
    public GameObject Setkind;

    //生成函数，分为在cell上生成和在高台上生成
    public void CreateSet()
    {
       //在selectcell非空且其未被占据（isblocked为false）时放置设施
        if (GameManager.selectcell!=null&& GameManager.selectcell.GetComponent<Cell>().isblocked == false)
        {
            //temp用于将即将生成的设施的z轴设置为-1（为使其显示在cell上）
            Vector3 temp = new Vector3(GameManager.selectcell.transform.position.x, GameManager.selectcell.transform.position.y, -1);
            Instantiate(Setkind, temp, Quaternion.identity);
            GameManager.selectcell.GetComponent<Cell>().isblocked = true;
        }

        //在selectplatform非空且其未被英雄棋子占据（isblocked为false）时放置设施,判断是否为英雄棋子的
        if (GameManager.selectplatform != null && GameManager.selectplatform.GetComponent<Platform>().isblocked == false&&this.CompareTag ("hero"))
        {
            //temp用于将即将生成的设施的z轴设置为-1（为使其显示在cell上）
            Vector3 temp = new Vector3(GameManager.selectplatform.transform.position.x, GameManager.selectplatform.transform.position.y, -1);
            Instantiate(Setkind, temp, Quaternion.identity);
            GameManager.selectplatform.GetComponent<Platform>().isblocked = true;
        }


    }
}
