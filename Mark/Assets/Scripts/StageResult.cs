using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class StageResult : MonoBehaviour {

    Text D,T,U,S;
    string Difficulty;
    static int UsedItem; // 한번 값 쓰고나면 매번 0으로 초기화시킴
    static int tt;

    public static int Total_UsedItem; // 초기화 없이 계속 값 저장
    public static int Total_tt;

    public static int countx = 0; // 스크립트 1번만 실행하기위함
    void Start()
    {
            tt = 0;
            UsedItem = 0; // 일단 초기화 시킨다음에
            D = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<Text>();
            T = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
            U = GameObject.FindGameObjectWithTag("UsedItem").GetComponent<Text>();
            S = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
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
            tt = Mathf.FloorToInt(Timer.time);
            UsedItem = Item.usedItem;// 다시 값 할당해줌

        /* 문제 발생 각 Text 마다 스크립트를 넣어놓으니
        똑같은 과정을 4번이나 반복해버린다 따라서 정적변수 countx 값을 넣어서 
        한 스테이지에서는 1번만 누적시키고 (count 1일때 정지) UIManager 에서 다시 카운트값 0으로 만들어줘서 각 스테이지 누적은 다시 되도록*/
        while (countx == 0)
        {
            Total_UsedItem += UsedItem; // 누적 아이템수
            Total_tt += tt;
            countx++;
        }
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
            U.text = "사용 아이템 수 : " + UsedItem;
        }
    
    // Update is called once per frame
    void Update()
    {

    }
}

