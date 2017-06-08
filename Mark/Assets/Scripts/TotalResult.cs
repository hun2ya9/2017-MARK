using UnityEngine;
using UnityEngine.UI;
using System.IO; // 파일 입출력 사용가능하게 해줌

using System.Collections;


/* 전반적인 설계
 * 일단 이거를 TotalResult와 합친다
 * GameClear() 는 최종 결과 보여주는거랑 같은부분이고
 * SaveScore() 는 파일에다가 결과 값 쓰는 메소드다.
 * GameClear_name 는 클리어 한 사람의 이름을 쓰는듯 하고 -> 이건 뺀다.
 * GameClear_score 는 점수를 쓰는듯 한데
 * 우리는 난이도, 시간, 사용아이템수 점수 4가지 요소가 있다
 * 따라서 파일 쓸때는 4줄을 쓰고 읽을때 4줄을 읽으면 된다. 
 */


public class TotalResult : MonoBehaviour
{
    /* 최종 결과창 = > 결과 출력과 동시에 파일에다가 write한다. */

    public Text Total_D, Total_T, Total_U, Total_S, Total_S1, Total_S2, Total_S3, Total_S4;
    string Difficulty; // 최종 난이도
    int tt; // 최종 시간
    int UsedItem; // 최종 사용 아이템수
    int score1;
    int score2;
    int score3;
    int score4;
    int score;
    int xx;
    public InputField inputName;
    static int count = 0; // 텍스트 오브젝트에 여러개 붙어있어서 여러번 실행되는 오류 막기위함
    string m_strPath = "Assets/Resources/"; // 텍스트의 경로
    //결과 저장, 순서는 난이도 시간 사용 아이템수 4가지 요소 한줄씩 저장
    public void GameClear_result(string Difficulty, int tt, int UsedItem, int score, string inputName)// 클리어시 결과 저장
    {
        FileStream f = new FileStream(m_strPath + "Result_Data.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

        string setInput = Difficulty +","+ tt + "," + UsedItem + "," + score + "," + inputName; // 어차피 ,이거 안읽으니까 
          writer.WriteLine(setInput); // 한 줄에 쉼표를 구분으로 4가지 요소를 저장하였다.
            writer.Close();
         
    }

    public void Save() {
        Debug.Log("저장되었습니다.");
        GameClear_result(Difficulty, tt, UsedItem, xx, inputName.text);

    }



    //public void GameClear_name(string strData) // 클리어시 플레이어 이름 입력
    //{
    //    FileStream f = new FileStream(m_strPath + "Data_name.txt", FileMode.Append, FileAccess.Write);
    //    StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
    //    writer.WriteLine(strData);
    //    writer.Close();
    //}



    void Start()
    {
        GameObject.Find("InGameBGM").GetComponent<AudioSource>().mute = true;

        //Total_D = GameObject.FindGameObjectWithTag("Total_Difficulty").GetComponent<Text>();
        //Total_T = GameObject.FindGameObjectWithTag("Total_Time").GetComponent<Text>();
        //Total_U = GameObject.FindGameObjectWithTag("Total_UsedItem").GetComponent<Text>();
        //Total_S1 = GameObject.FindGameObjectWithTag("Score1").GetComponent<Text>();
        //Total_S2 = GameObject.FindGameObjectWithTag("Score2").GetComponent<Text>();
        //Total_S3 = GameObject.FindGameObjectWithTag("Score3").GetComponent<Text>();
        //Total_S4 = GameObject.FindGameObjectWithTag("Score4").GetComponent<Text>();
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
        Total_S1.text = "1st Score :" + StageResult.s_Score1;
        Total_S2.text = "2st Score :" + StageResult.s_Score2;
        Total_S3.text = "3st Score :" + StageResult.s_Score3;
        Total_S4.text = "4st Score :" + StageResult.s_Score4;
        xx = StageResult.s_Score1 + StageResult.s_Score2 + StageResult.s_Score3 + StageResult.s_Score4;
        Total_S.text = "Total Score :" + xx;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
