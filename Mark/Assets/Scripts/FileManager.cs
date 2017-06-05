using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

/* 설명
 * 우리는 난이도, 시간, 사용아이템수 점수 4가지 요소가 있다.
 * 게임 클리어시 TotalResult 클래스에서 텍스트 한 줄에 쉼표를 구분으로 4가지 요소를 쓴다.
 * 따라서 불러올때는 한 줄씩 읽어오는데 쉼표를 때고 요소별로 구분이 되게 해야한다.
 */



public class FileManager : MonoBehaviour
{

    Text Rank_D, Rank_T, Rank_U, Rank_S1, Rank_S2, Rank_S3;

    string s_Difficulty;
    public static string s_Time; // 누적 시간
    public static string s_UsedItem; // 누적 아이템 사용 수
    public static string s_Score; // 누적 점수
    string m_strPath = "Assets/Resources/";
    static ArrayList easy = new ArrayList();
    static ArrayList normal = new ArrayList();
    static ArrayList hard = new ArrayList();

    int[] split = new int[100]; // 4가지 요소니까 쉼표 3개의 위치를 저장하는 배열
    string[,] data = new string[100, 4];

    /* 설계
   2. 한 줄씩 읽어와서 각각 문자배열에 넣는다. 요소가 4개니까 이차원배열 [][4] 요런식으로
   [0][] [1][]*/

    /* 랭킹 순위 올리는 방법
     Easy
     Normal
     Hard 로 구분지어서
     각각 3등까지 높은 점수로 정렬한다.
     점수를 easy, normal, hard 배열에 바로 집어넣고
     추가 정보가 들어올 때 마다 계속 높은게 앞으로 오도록 정렬시켜서
     무조건 0,1,2번 배열이 text로 출력되도록
     
        랭킹에는 점수만 반영할거니까 점수만 띄우고
        나머지는 업적이랑 관련해서 따로 계속 누적시키는걸로
         */
    public void DataLoad()
    {
        FileStream f = new FileStream(m_strPath + "Result_Data.txt", FileMode.Open);
        StreamReader sr = new StreamReader(f);
        sr.BaseStream.Seek(0, SeekOrigin.Begin);// 파일을 읽기 시작

        int k = 0;
        while (sr.Peek() > -1)
        {

            for (int x = 0; x < 100; x++)
            {
                //탈출문을 어떻게 만들까? 읽어올게 없다거나 배열에 넣을게 없다거나 하면 ..
                string ReadLine = sr.ReadLine(); // 한줄씩 읽어온다.
                if (ReadLine == null)
                    break;
                char[] arrChar = ReadLine.ToCharArray(); //문자 하나씩 배열에 집어넣는다.



                for (int i = 0; i < arrChar.Length; i++)
                {
                    if (arrChar[i] == ',')
                    { // 해당 문자가 쉼표면 그 배열숫자 기억
                        split[k] = i;
                        k++;
                    } // split 배열은 무조건 한 줄에 3개로 고정이기 때문에 이렇게 쓸 수 있다.
                }
                //이차원 배열에다가 4개의 요소를 집어넣는다. 각 줄마다 정확히 배열 3개만큼씩 차이남
                data[x, 0] = ReadLine.Substring(0, split[0 + 3 * x]); // 난이도
                data[x, 1] = ReadLine.Substring(split[0 + 3 * x] + 1, split[1 + 3 * x] - split[0 + 3 * x] - 1); // 시간
                data[x, 2] = ReadLine.Substring(split[1 + 3 * x] + 1, split[2 + 3 * x] - split[1 + 3 * x] - 1); // 사용 아이템
                data[x, 3] = ReadLine.Substring(split[2 + 3 * x] + 1); // 점수

                s_Time += data[x, 1]; // 누적 
                s_UsedItem += data[x, 2]; // 누적
                s_Score += data[x, 3]; // 누적


                if (data[x, 0] == "Easy") // 난이도 별로
                {
                    easy.Add(int.Parse(data[x, 3]));
                }
                else if (data[x, 0] == "Normal")
                {
                    normal.Add(int.Parse(data[x, 3]));
                }
                else if (data[x, 0] == "Hard")
                {
                    hard.Add(int.Parse(data[x, 3]));          
                }

                // 한 줄 읽고 분리시키고 분리시킨 점수를 배열에 저장하고 바로 소트돌린다.
                easy.Sort(); // 오름차순
                easy.Reverse(); // 거꾸로하면 내림차순
                normal.Sort(); // 오름차순
                normal.Reverse();
                hard.Sort(); // 오름차순
                hard.Reverse();
                // 소트 시키고 나면 그녀석의 줄 번호를 어떻게 기억해야할지 모르겠다..              
            }
        }
            sr.Close();
            f.Close();
        
        // 처음 게임 시작하면 기록이 없어서 = ( 값이 없을 수도 있어서 ) 없는 값은 0으로 출력하기 위함
        if (easy.Count == 1)
        {
            easy.Add(0);
            easy.Add(0);
        }
        else if (easy.Count == 2)
        {
            easy.Add(0);
        }

        if (normal.Count == 1)
        {
            normal.Add(0);
            normal.Add(0);

        }
        else if (normal.Count == 2)
        {
            normal.Add(0);
        }

        if (hard.Count == 1)
        {
            hard.Add(0);
            hard.Add(0);

        }
        else if (hard.Count == 2)
        {
            hard.Add(0);
        }
        
    }

    void Start()
    {

        //Rank_D = GameObject.FindGameObjectWithTag("Rank_Difficulty").GetComponent<Text>();
        //Rank_T = GameObject.FindGameObjectWithTag("Rank_Time").GetComponent<Text>();
       // Rank_U = GameObject.FindGameObjectWithTag("Rank_UsedItem").GetComponent<Text>();
        Rank_S1 = GameObject.FindGameObjectWithTag("Rank_Score").GetComponent<Text>();
        Rank_S2 = GameObject.FindGameObjectWithTag("Rank_Score2").GetComponent<Text>();
        Rank_S3 = GameObject.FindGameObjectWithTag("Rank_Score3").GetComponent<Text>();

        DataLoad();
        

        // Easy 난이도
        // Rank_D.text = "난이도 :" + (data[zero,0]+ "\r\n" + data[first, 0] + "\r\n" + data[second, 0]);
        // Rank_T.text = "시간 :" + (data[zero, 1] + "\r\n" + data[first, 1] + "\r\n" + data[second, 1]);
        // Rank_U.text = "사용 아이템수 :" + (data[zero, 2] + "\r\n" + data[first, 2] + "\r\n" + data[second, 2]);

        //우와 텍스트 출력하면 값이 변하는 마법이ㅎㅎ
        Rank_S1.text = easy[0] + "      " + easy[1] + "     " + easy[2];
        Rank_S2.text = normal[0] + "        " + normal[1] + "       " + normal[2];
        Rank_S3.text = hard[0] + "        " + hard[1] + "       " + hard[2];

    }



}