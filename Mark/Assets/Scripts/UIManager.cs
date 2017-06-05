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
        GameObject.FindGameObjectWithTag("OpenBGM").GetComponent<AudioSource>().mute = false;
        GameObject.Find("InGameBGM").GetComponent<AudioSource>().mute = true;
        /*매뉴로 돌아간다는 말은
         게임 시작 전 / 시작 후로 나뉘는데
         시작 후에는 죽은 경우, 클리어시 2가지 종류가 있다.
         게임 죽고 다시 실행하면 이전 데이터가 남아있는 버그가 발생한다. = 초기화 작업 추가
         1. 스테이지 초기화
         2. 아이템 초기화
         3. 누적 값 초기화 = 아마 계속 누적되고 있을거임
          */
        stage = 1;
        OBD = false;
        EBD = false;
        K = false;
        StageResult.Total_tt = 0;
        StageResult.Total_UsedItem = 0;
        Player.t = 0; // 지나온 길 초기화
        SceneManager.LoadScene("Opening UI");
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
    public void Opening()
    {
        SceneManager.LoadScene("Opening UI");
    }
    public void Explain_UI()
    {
        SceneManager.LoadScene("Game Explain UI 1");
    }
    public void Explain_Item()
    {
        SceneManager.LoadScene("Game Explane - Item");
    }
    public void Explain_Ect()
    {
        SceneManager.LoadScene("Game Explain - Ect");
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
        Player.t = 0; // 지나온 길 초기화
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
        Screen.SetResolution(1024, 768, true); //해상도 설정

    }

    // Update is called once per frame
    void Update()
    {

    }
}