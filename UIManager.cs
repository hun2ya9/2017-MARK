using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public void Menu()
    {
        SceneManager.LoadScene("Opening UI");
    }
    public void play()
    {
        SceneManager.LoadScene("Game Explane UI");
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
        SceneManager.LoadScene("Easy_Stage1");
    }
    public void Normal()
    {
        SceneManager.LoadScene("Normal_Stage1");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Hard_Stage1");
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