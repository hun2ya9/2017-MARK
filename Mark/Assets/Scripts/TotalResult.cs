using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class TotalResult : MonoBehaviour
{
    /* 최종 결과창*/
    Text Total_D, Total_T, Total_U, Total_S;
    string Difficulty;
    int tt;
    int UsedItem;

    void Start()
    {
        Total_D = GameObject.FindGameObjectWithTag("Total_Difficulty").GetComponent<Text>();
        Total_T = GameObject.FindGameObjectWithTag("Total_Time").GetComponent<Text>();
        Total_U = GameObject.FindGameObjectWithTag("Total_UsedItem").GetComponent<Text>();
        //Total_S = GameObject.FindGameObjectWithTag("Total_Score").GetComponent<Text>();
        if (UIManager.level == 1)
        {
            Difficulty = "Easy";
        }
        else if (UIManager.level == 2)
        {
            Difficulty = "Normal";
        }
        else if (UIManager.level == 3)
        {
            Difficulty = "Hard";
        }

        tt += Mathf.FloorToInt(StageResult.Total_tt); // 누적 시간 불러옴
        UsedItem += StageResult.Total_UsedItem; // 누적 아이템 수 불러옴
        float minute = Mathf.FloorToInt(tt / 60);
        string second = tt.ToString();
        string ResetSecond = (tt - 60 * Mathf.FloorToInt(tt / 60)).ToString();

        if (tt < 60)
        {
            Total_T.text = "플레이 시간 : 00분" + second + "초";
        }
        else if (tt >= 60)
        {
            Total_T.text = "플레이 시간 : " + minute + "분" + ResetSecond + "초";
        }
        
        Total_D.text = "난이도 :" + Difficulty;
        Total_U.text = "사용 아이템 수 : " + UsedItem;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
