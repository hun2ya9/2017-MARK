using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static int level;
    public static int stage = 1; //첫 스테이지는 1번
    public static bool OBD, EBD, K; // 스테이지 넘어가도 아이템 유지 되도록

    public void Menu()
    {
        SceneManager.LoadScene("Opening UI");
    }
    public void play()
    {
        SceneManager.LoadScene("Game Explain UI");
    }

    public void Difficulty()
    {
        SceneManager.LoadScene("Difficulty Choice UI");
    }
    public void Setting()
    {
        SceneManager.LoadScene("Sound Setting UI");
    }

    public void Ranking()
    {
        SceneManager.LoadScene("Ranking UI");
    }
    public void Exit()
    {
        SceneManager.LoadScene("Exit Confirm UI");
    }

    public void ExitSure() {
        Application.Quit();
    }

    public void Easy()
    {
        level = 1;
        SceneManager.LoadScene("MAP1");

      
    }
    
    public void Normal()
    {
        level = 2;
        SceneManager.LoadScene("MAP1");
    }
    public void Hard()
    {
        level = 3;
        SceneManager.LoadScene("MAP1");
    }
    public void NextStage()
    {


        StageResult.countx = 0; // 다른 스테이지 간에는 출력해야되므로
        if (UIManager.level == 1)
        {
            level = 1;
        }
        else if (UIManager.level == 2)
        {
            level = 2;
        }
        else
        {
            level = 3;
        }

        stage++; // 다음단계 넘어갈땐 stage값 증가
        if (UIManager.stage == 5)
        {


            SceneManager.LoadScene("Total Result UI");
        }
        else
        {
            SceneManager.LoadScene("MAP1");
        }
    }


    public void GameOver()
    {
        SceneManager.LoadScene("Game Over UI'");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}