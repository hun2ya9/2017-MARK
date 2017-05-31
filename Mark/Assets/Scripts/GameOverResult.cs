using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GameOverResult : MonoBehaviour
{
    /* 최종 결과창 TotalResult 랑 같이 쓰려고 했지만 좀더 독립적인 특성이 있다.
     예를 들어 TotalResult는 StageResult에서 누적값을 가져오는 형식인데
     GameOver은 플레이어가 1스테이지가 끝나기도 전에 죽어버리면 StageResult값을 가져오지도 못한다. (한번도 실행된 적이 없기 때문)
    값을 누적해서 계속 더하다가 호출 되는 순간 그값을 보여주는 형식으로 만들자
     */
    Text GameOver_D, GameOver_T, GameOver_U, GameOver_S;
    string Difficulty;
    static int GameOver_UsedItem;
    static int GameOver_tt; // 누적 값

    int UsedItem;
    int tt; // 스테이지 마다 더할 값

    public static int countx = 0; // 스크립트 1번만 실행하기위함
    void Start()
    {
        GameOver_D = GameObject.FindGameObjectWithTag("GameOver_Difficulty").GetComponent<Text>();
        GameOver_T = GameObject.FindGameObjectWithTag("GameOver_Time").GetComponent<Text>();
        GameOver_U = GameObject.FindGameObjectWithTag("GameOver_UsedItem").GetComponent<Text>();
        //GameOver_S = GameObject.FindGameObjectWithTag("GameOver_Score").GetComponent<Text>();

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


        /* 출력 방법을 2가지로 나눠야 한다.
         * 1스테이지 : stageResult가 실행 된적이 없으므로 그냥 현재 값 불러오면됨
         2스테이지 이상 : 1스테이지 지나가면 값이 초기화 되기 때문에 누적 시켜야됨
         그 말은 stageResult 에서 누적시킨 값을 가져와야 된다는 뜻.*/

        if (UIManager.stage == 1)
        {
            tt = Mathf.FloorToInt(Timer.time); // 게임 종료되는 즉시 시간 저장
            float minute = Mathf.FloorToInt(tt / 60);
            string second = tt.ToString();
            string ResetSecond = (tt - 60 * Mathf.FloorToInt(tt / 60)).ToString();

            if (tt < 60)
            {
                GameOver_T.text = "플레이 시간 : 00분" + second + "초";
            }
            else if (tt >= 60)
            {
                GameOver_T.text = "플레이 시간 : " + minute + "분" + ResetSecond + "초";
            }
            UsedItem = Item.usedItem;

            GameOver_D.text = "난이도 :" + Difficulty;
            GameOver_U.text = "사용 아이템 수 : " + UsedItem;
        }
        else // 스테이지 2 이상일때
        {
            tt += Mathf.FloorToInt(StageResult.Total_tt + Timer.time); // 누적 시간과 현재 시간 합
            UsedItem += (StageResult.Total_UsedItem + Item.usedItem); // 누적 아이템 수와 현재 아이템 수 합
            float minute = Mathf.FloorToInt(tt / 60);
            string second = tt.ToString();
            string ResetSecond = (tt - 60 * Mathf.FloorToInt(tt / 60)).ToString();

            if (tt < 60)
            {
                GameOver_T.text = "플레이 시간 : 00분" + second + "초";
            }
            else if (tt >= 60)
            {
                GameOver_T.text = "플레이 시간 : " + minute + "분" + ResetSecond + "초";
            }
            GameOver_D.text = "난이도 :" + Difficulty;
            GameOver_U.text = "사용 아이템 수 : " + UsedItem;

        }
    
    }
        // Update is called once per frame
        void Update()
        {

        }
    
}
