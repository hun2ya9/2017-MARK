using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 업적 구현
 게임 클리어시 업적 파일에다가 값을 누적 시켜서 저장할 수 있지않을까?
 파일 가져오는거만 어떻게 할 수 있으면 하는데
 */
// 누적 값 가져왔음

public class achievement : MonoBehaviour
{
    public Text TT, TU, TS;
    int a_time;
    int a_UsedItem;
    int a_Score;
    static int point;

    public GameObject Tb,Ts,Tg;
    public GameObject Ub, Us, Ug;
    public GameObject Sb, Ss, Sg;

    void Start()
    {
        point = 0; // 스크립트 호출때마다 0으로 초기화

        a_time = FileManager.s_Time; // 누적 시간 = 초 단위로 기록될거임
        a_UsedItem = FileManager.s_UsedItem; // 누적 아이템 사용
        a_Score = FileManager.s_Score; // 누적 점수
        

        Achieve();
        TT.text = " " + a_time;
        TU.text = " " + a_UsedItem;
        TS.text = " " + a_Score;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Achieve() { // 일정 수를 넘으면 포인트 획득하는걸로

        if (a_time > 3600)
        {
            Tb.SetActive(true);
        }
        if (a_time > 7200)
        {
            Ts.SetActive(true);

        }
        if (a_time > 10800)
        {
            Tg.SetActive(true);

        }
        if (a_UsedItem > 50)
        {
            Ub.SetActive(true);

        }
        if (a_UsedItem > 150)
        {
            Us.SetActive(true);

        }
        if (a_UsedItem > 350)
        {
            Ug.SetActive(true);

        }
        if (a_Score > 1000)
        {
            Sb.SetActive(true);

        }
        if (a_Score > 3000)
        {
            Ss.SetActive(true);

        }
        if (a_Score > 5000)
        {
            Sg.SetActive(true);

        }
    }

    /* 상점을 구현한다면
    버튼을 눌렀을때 해당 포인트보다 크면 그 포인트만큼 차감하고
    뉴 플레이어(=배)를 획득할 수 있도록
    
     */
    void store() { // 해당 on click 에 추가
        if (point > 10) {
            point -= 10;
            // 플레이어의 sprite 교체 뭐 이런식으로
            GameObject.Find("Player").GetComponent<Sprite>();
            // 여기다 새 이미지 입히면 되지 않을까?
        }
    }

}
