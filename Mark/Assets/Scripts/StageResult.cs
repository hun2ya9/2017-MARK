using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class StageResult : MonoBehaviour {

    Text D,T,U,S;
    string Difficulty;
    //string Time = Timer.time.ToString();
    string UseItem = Item.useItem.ToString();

    void Start()
    {
        D = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<Text>();
        T = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        U = GameObject.FindGameObjectWithTag("UsedItem").GetComponent<Text>();
        S = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        if (UIManager.level == 1) {
            Difficulty = "Easy";
        }
        else if (UIManager.level == 2) {
            Difficulty = "Normal";
        }
        else if (UIManager.level == 3) {
            Difficulty = "Hard";
        }

        int tt = Mathf.FloorToInt(Timer.time);
        float minute = Mathf.FloorToInt(tt / 60);
        string second = tt.ToString();
        string ResetSecond = (tt - 60 * Mathf.FloorToInt(tt / 60)).ToString();

        if (tt < 60)
        {
            T.text = "플레이 시간 : 00분" + second + "초";
        }
        else if (tt >= 60)
        {
            T.text = "플레이 시간 : " + minute + "분" + ResetSecond + "초";
        }


        D.text = "난이도 :" + Difficulty;
        //T.text = "플레이 시간 : " + Time;
        U.text = "사용 아이템 수 : " + UseItem;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
