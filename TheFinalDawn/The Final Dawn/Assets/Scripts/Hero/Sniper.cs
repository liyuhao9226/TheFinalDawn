using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    /*  
    普通射手：阻挡数为1；攻击范围为3；一次攻击数为1；
    升级后
    散射：一次攻击目标为3。
    狙击：攻击距离加4。
    */
    public void Sanshe()
    {
        this.transform.GetChild(0).GetComponent<HeroAttack>().onceattacknumber = 3;

        sanshebutton.SetActive(false);
        jujibutton.SetActive(false);
        this.GetComponent<HeroBasic>().buttonactive = false;
        this.GetComponent<HeroBasic>().destorybutton.SetActive(false);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        this.transform.GetComponent<SpriteRenderer>().sprite = this.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<Animator>().runtimeAnimatorController = this.transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController;


        haveupdated = true;
    }
    public void Juji()
    {
        this.GetComponent<HeroBasic>().attackrange += 4;
        this.GetComponent<HeroBasic>().AttackRangeChange();
        sanshebutton.SetActive(false);
        jujibutton.SetActive(false);
        this.GetComponent<HeroBasic>().buttonactive = false;
        this.GetComponent<HeroBasic>().destorybutton.SetActive(false);

        this.transform.GetComponent<SpriteRenderer>().sprite = this.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite;
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
    public GameObject sanshebutton;
    public GameObject jujibutton;
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
            sanshebutton.SetActive(true);
            jujibutton.SetActive(true);
           // this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            buttonactive = true;

        }
        else
        {
            sanshebutton.SetActive(false);
            jujibutton.SetActive(false);
          //  this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            buttonactive = false;
        }

        }
       
    }

}
