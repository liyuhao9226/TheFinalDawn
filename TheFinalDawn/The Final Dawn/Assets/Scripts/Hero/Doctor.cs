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
public class Doctor : MonoBehaviour
{
   
    void Start()
    {
        //将治疗属性设置为true
        this.transform.GetChild(0).GetComponent<HeroAttack>().zhiliao = true;

    }

    // Update is called once per frame
    void Update()
    {

     
    


    }
    /*
     
         
         */
    public void Qunyu()
    {
        //调整数值部分
        this.GetComponent<HeroBasic>().atk /= 2;
        //将一次攻击数置为99表示群体治疗
        this.transform.GetChild(0).GetComponent<HeroAttack>().onceattacknumber = 99;
        //后续处理部分
        qunyubutton.SetActive(false);
        fuzhubutton.SetActive(false);
        this.GetComponent<HeroBasic>().buttonactive = false;
        this.GetComponent<HeroBasic>().destorybutton.SetActive(false);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        this.GetComponent<Animator>().runtimeAnimatorController = this.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController;

        haveupdated = true;

    }
    public void Fuzhu()
    {
        //调整数值部分
        this.GetComponent<HeroBasic>().atk=0; this.GetComponent<HeroBasic>().fuzhu = true; this.transform.GetChild(0).GetComponent<HeroAttack>().fuzhu = true;
        if (this.transform.GetChild(0).GetComponent<HeroAttack>().heroList.Count > 0)
        {
            foreach (var item in this.transform.GetChild(0).GetComponent<HeroAttack>().heroList)
        {
            item.GetComponent<HeroBasic>().atk += 2;
            item.GetComponent<HeroBasic>().def += 2;
                //显示增益光环
            item.GetComponent<HeroBasic>().zengyi.SetActive(true);
        } 
        }
       
        //隐藏飞行道具的图片
        //   this.transform.GetChild(0).GetComponent<HeroAttack>().projectile.transform.GetComponent<SpriteRenderer>().enabled = false; 

        //后续处理部分
        qunyubutton.SetActive(false);
        fuzhubutton.SetActive(false);
        this.GetComponent<HeroBasic>().buttonactive = false;
        this.GetComponent<HeroBasic>().destorybutton.SetActive(false);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //更换动画
        this.GetComponent<Animator>().runtimeAnimatorController = this.transform.GetChild(3).GetComponent<Animator>().runtimeAnimatorController;


        haveupdated = true;

    }



    //用于显示信息
    public GameObject info1;
    public GameObject info2;

    public void Showinfo1()
    {
        info1.SetActive(true);
    }
    public void Showinfo2()
    {
        info2.SetActive(true);
    }
    public void Closeinfo1()
    {
        info1.SetActive(false);
    }
    public void Closeinfo2()
    {
        info2.SetActive(false);
    }





    //用于显示/关闭操作按钮
    public GameObject qunyubutton;
    public GameObject fuzhubutton;
    bool buttonactive;
    //升级后升级按钮不再显示
    bool haveupdated = false;
    private void OnMouseDown()
    {
        //关闭信息显示
        Closeinfo1(); Closeinfo2();
        if (!haveupdated)
        {

            if (buttonactive != true)
            {
                qunyubutton.SetActive(true);
                fuzhubutton.SetActive(true);
                // this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                buttonactive = true;

            }
            else
            {
                qunyubutton.SetActive(false);
                fuzhubutton.SetActive(false);
                //  this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                buttonactive = false;
            }

        }

    }





}
