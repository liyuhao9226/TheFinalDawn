using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guarder : MonoBehaviour
{

  

   
    void Start()
    {
        //将近战属性设置为true
        this.transform.GetChild(0).GetComponent<HeroAttack>().jinzhan = true;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  
     普通铁卫：阻挡数为x；攻击范围为自身格与身前一格；一次攻击数为1；
     升级后
     重装铁卫：阻挡数加倍且攻击减少，防御增加。
     战术大师：攻击、攻速增加且变为一次可攻击3个目标。
     */
    public void Zhongzhuang()
    {
        //调整数值部分
        this.GetComponent<HeroBasic>().blocknumber*= 2;
        this.GetComponent<HeroBasic>().atk -= 2;
        this.GetComponent<HeroBasic>().def += 2;


        //后续处理部分
        zhongzhuangbutton.SetActive(false);
        zhanshubutton.SetActive(false);
        this.GetComponent<HeroBasic>().buttonactive = false;
        this.GetComponent<HeroBasic>().destorybutton.SetActive(false);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        this.GetComponent<Animator>().runtimeAnimatorController = this.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController;

        haveupdated = true;

    }
    public void Zhanshu()
    {
        this.transform.GetChild(0).GetComponent<HeroAttack>().onceattacknumber = 3;
        this.GetComponent<HeroBasic>().atk += 2;
        this.transform.GetChild(0).GetComponent<HeroAttack>().attackspeed+=1;
        this.GetComponent<HeroBasic>().blocknumber -=1;
        this.transform.GetChild(0).GetComponent<HeroAttack>().jinzhan = false;



        zhongzhuangbutton.SetActive(false);
        zhanshubutton.SetActive(false);
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
    public GameObject zhongzhuangbutton;
    public GameObject zhanshubutton;
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
                zhongzhuangbutton.SetActive(true);
                zhanshubutton.SetActive(true);
                // this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                buttonactive = true;

            }
            else
            {
                zhongzhuangbutton.SetActive(false);
                zhanshubutton.SetActive(false);
                //  this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                buttonactive = false;
            }

        }

    }









}
