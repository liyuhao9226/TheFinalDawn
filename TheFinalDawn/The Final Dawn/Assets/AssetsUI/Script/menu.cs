using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public Animator anim;

    public  void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameObject.Find("Canvas/menu/game/load").SetActive(false);
        GameObject.Find("Canvas/menu/game/newgame").SetActive(true);
        GameObject.Find("Canvas/menu/game/read").SetActive(true);
        GameObject.Find("Canvas/menu/game").SetActive(false);
    }
    public  void  QuitGame()
    {
        Application.Quit();
    }
    public void EnableShezhi()
    {
        GameObject.Find("Canvas/menu/set").SetActive (true );
    }
    public void EnableYouxi()
    {
        GameObject.Find("Canvas/menu/game").SetActive(true);
    }
    public void EnablGengduo()
    {
        GameObject.Find("Canvas/menu/more").SetActive(true);
    }
    public void DisableShezhi()
    {
        GameObject.Find("Canvas/menu/set").SetActive(false);
    }
    public void DisableYouxi()
    {
        GameObject.Find("Canvas/menu/game").SetActive(false);
    }
    public void DisableGengduo()
    {
        GameObject.Find("Canvas/menu/more").SetActive(false);
    }
    public  void Load()
    {
        GameObject.Find("Canvas/menu/game/load").SetActive(true);
        GameObject.Find("Canvas/menu/game/newgame").SetActive(false);
        GameObject.Find("Canvas/menu/game/read").SetActive(false);
        anim.SetBool ("loading", true);
    }
    public void Loading()
    {
        Invoke("PlayGame", 2f);
    }
}
