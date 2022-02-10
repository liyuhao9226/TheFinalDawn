using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu1 : MonoBehaviour
{
    public Animator anima;

    public void GoGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameObject.Find("Canvas/View/Viewport/Content/c1/zhuan").SetActive(false);
        GameObject.Find("Canvas/View/Viewport/Content/c1/jian").SetActive(true);
       
    }
    public void BackGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void EnShezhi()
    {
        GameObject.Find("Canvas/menu/set").SetActive(true);
    }
   
    public void DisShezhi()
    {
        GameObject.Find("Canvas/menu/set").SetActive(false);
    }
   
    public void Wait()
    {
        GameObject.Find("Canvas/View/Viewport/Content/c1/zhuan").SetActive(true);
        GameObject.Find("Canvas/View/Viewport/Content/c1/jian").SetActive(false);
        anima.SetBool("waiting", true);
    }
    public void Waiting()
    {
        Invoke("GoGame", 1.5f);
    }
}
