using UnityEngine;
using System.Collections;

/* 업적 구현
 게임 클리어시 업적 파일에다가 값을 누적 시켜서 저장할 수 있지않을까?
 파일 가져오는거만 어떻게 할 수 있으면 하는데
 */
 // 누적 값 가져왔음

public class achievement : MonoBehaviour
{
    string a_time;
    string a_UsedItem;
    string a_Score;
    static int point;
    // Use this for initialization
    void Start()
    {
        a_time = FileManager.s_Time; // 누적 시간 = 초 단위로 기록될거임
        a_UsedItem = FileManager.s_UsedItem; // 누적 아이템 사용
        a_Score = FileManager.s_Score; // 누적 점수
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Achieve() { // 일정 수를 넘으면 포인트 획득하는걸로

        if (int.Parse(a_time) > 3600)
        {
            point++;
        }
        if (int.Parse(a_UsedItem) > 100){
            point++;
        }
        if (int.Parse(a_Score) > 1000) {
            point++;
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
