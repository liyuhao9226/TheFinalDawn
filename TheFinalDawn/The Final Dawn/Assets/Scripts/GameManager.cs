using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/****************************************************************
 脚本描述
 
 功能：

 内接外接口：
 
 外接内接口：

 注意事项：

****************************************************************/
public class GameManager : MonoBehaviour
{
    public GameObject selectcell;
    //用于定位攻击范围
    public GameObject selectattackcell;
    public GameObject selectplatform;
    private GameObject []enemies;
    public GameObject bociutton;
    //敌人数量，为0时显示波次按钮
    public int enemynum=0;
    public bool wallsetting;
    //找到核心
    public Core Core;
   //找到过场动画
    public Animator BattleStart;

    //表明战斗进行
    public bool battleon;
    //设施数组。依次表示铁卫，治疗，狙击，高台，陷阱的可防止数量
    public int []setnum = { 2, 2, 2, 2, 3 };
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(setnum[2]);
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        Core = GameObject.Find("Core").GetComponent<Core>();
        BattleStart = GameObject.Find("BattleStart").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (enemynum==0&&wavenum!=0) { bociutton.SetActive(true);PathOpen(); }

        if (enemynum ==0&&wavenum == 1&& Core.hp>0) { BattleStart.SetTrigger("Wave2"); battleon = false; }

        if (enemynum == 0 && wavenum == 2 && Core.hp > 0) { BattleStart.SetTrigger("Wave3"); battleon = false; }

        if (enemynum == 0 && wavenum == 3 && Core.hp > 0) { BattleStart.SetTrigger("Victory"); battleon = false; wavenum++; }

        if (Core.hp <= 0&&battleon) { BattleStart.SetTrigger("Lose"); battleon = false; }

        if(isfail ||isvictory) { shezhilanend.SetActive (true); }
    }

    public void EnemyFindPath()
    {
        foreach(var enemy in enemies)
        {
            enemy.GetComponent<PathManager>().FindPath();
            enemy.GetComponent<PathManager>().start = true;
        }
    }


    public int wavenum=0;
    //怪物预制体
    public GameObject pig;
    public GameObject fly;
    public GameObject bao;
    //生成波次
    public void WaveGenerate()
    {
        battleon = true;
        bociutton.SetActive(false);
        PathClose();
        int i = -10;
        Vector2 generateposition1=new Vector2(i,5);
        Vector2 generateposition2 = new Vector2(11, -6);
        //每波从左上角生成3只依次进入的猪
        for (int j = 0; j < 3; j++)
        { Instantiate(pig, generateposition1, Quaternion.identity);i--; generateposition1 =  new Vector2(i, 5); enemynum++; }
        //波次数量在至少生成一个敌人后增加避免波次胜利和最终胜利的判断冲突
        wavenum++;
        switch (wavenum)
        {
            //第2，3波右侧生成1龙和2猪
            case 2:
                Instantiate(fly, generateposition2, Quaternion.identity); enemynum++;
                generateposition2 = new Vector2(12, -6);      
                Instantiate(pig, generateposition2, Quaternion.identity); enemynum++;
                generateposition2 = new Vector2(13, -6);
                Instantiate(pig, generateposition2, Quaternion.identity); enemynum++;
                break;

               
            case 3:
                Instantiate(fly, generateposition2, Quaternion.identity); enemynum++;
                generateposition2 = new Vector2(12, -6);
                Instantiate(pig, generateposition2, Quaternion.identity); enemynum++;
                generateposition2 = new Vector2(13, -6);
                Instantiate(pig, generateposition2, Quaternion.identity); enemynum++;
                //第3波从左侧生成1豹2猪
                generateposition1 = new Vector2(-15, 5);
                Instantiate(bao, generateposition1, Quaternion.identity); enemynum++;
                generateposition1 = new Vector2(-17, 5);
                Instantiate(pig, generateposition1, Quaternion.identity); enemynum++;
                generateposition1 = new Vector2(-18, 5);
                Instantiate(pig, generateposition1, Quaternion.identity); enemynum++;
                break;
        }             
    }

    //波次开始后关闭路径显示
    void PathClose()
    {
        if (this.GetComponent<PathShower>().ClosedList != null)
        {
            foreach (var item in this.GetComponent <PathShower>().ClosedList)
        {
            item.GetComponent<Cell>().pathcell.SetActive(false);
        }
        }
        if (this.GetComponent<PathShowerNorthEast>().ClosedList != null)
        {
            foreach (var item in this.GetComponent<PathShowerNorthEast>().ClosedList)
            {
                item.GetComponent<Cell>().pathcell.SetActive(false);
            }
        }

    }
    //波次结束后开启路径显示
    void PathOpen()
    {
        if (this.GetComponent<PathShower>().ClosedList != null)
        {
            foreach (var item in this.GetComponent<PathShower>().ClosedList)
            {
                item.GetComponent<Cell>().pathcell.SetActive(true);
            }
        }
        if (this.GetComponent<PathShowerNorthEast>().ClosedList != null)
        {
            foreach (var item in this.GetComponent<PathShowerNorthEast>().ClosedList)
            {
                item.GetComponent<Cell>().pathcell.SetActive(true);
            }
        }

    }

    //设置控制部分
    public GameObject shezhilan;

    public void SetOpen()
    {
        shezhilan.SetActive(true);
        Time.timeScale = 0;
    }
    public void SetClose()
    {
        shezhilan.SetActive(false);
        Time.timeScale = 1;
    }
    public void Tuichu()
    {
        SceneManager.LoadScene(1);
    }
    //退出至主菜单
    public void Tuichuzhu()
    {
        SceneManager.LoadScene(0);
    }
    //重来
    public void TryAgain()
    {
        SceneManager.LoadScene(2);
    }
    //战斗胜利或失败后的部分
    public GameObject shezhilanend;
    public bool isvictory;
    public bool isfail;
}
