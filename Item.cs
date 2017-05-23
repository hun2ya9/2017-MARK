using System.Collections.Generic; // 이걸 써야 List 사용가능
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item : MonoBehaviour
{
    string[] items = new string[4];
    string[] Inventory = new string[3];
    
    /*
    아이템 랜덤 배치, 겹쳐도 상관없는데 안보이게 할꺼고
     충돌처리해서 해당 위치가면 collider 발동해서 하단의 아이템창에 추가하는식으로
     아이템 종류는 랜덤인데 아이템 창에 없는 아이템 뜨도록 => 배열에 0번부터 확인
 */
    void Start()
    {
    }

    void Update()
    {

    }



    /*아이템 먹는거 설계
     아이템 종류 배열에 저장
     플레이어가 아이템과 충돌시 랜덤 숫자 Random.Range(0,4) 으로 아이템 종류 배열에서 아무거나 가져와서
     해당 아이템이 존재하는가? 존재하면 다시값 할당받아서 없는 아이템 먹을때까지 반복
         */

    bool exist = false; // 인벤토리에 먹은 아이템 있는지확인 = 기본값 없는걸로(false)
    public Item()
    {
        items[0] = "OneBlockDetector"; // 한 블록 탐지기
        items[1] = "EightBlockDetector"; // 8방위 탐지기 
        items[2] = "Knight"; // 나이트
        items[3] = "RandomTeleport"; // 랜덤으로 아무대나 보냄
    }

    void OnTriggerEnter2D(Collider2D coll) // player가 item에 부딪힐때
    {
        int random;
        if (coll.gameObject.tag == "Item") // 부딪힌게 아이템 일때
        {
            random = Random.Range(0, 4); // 아이템 랜덤으로 먹어짐

            for (int i = 0; i < 3; i++) // 없는 아이템 먹을때까지 반복
            {
                if (exist == true) // 아이템 존재할시
                {
                    random = Random.Range(0, 4); // 다시 할당
                    exist = false;
                }

                for (int j = 0; j < 3; j++) // 랜덤으로 먹은 아이템이
                {
                    if (items[random] == Inventory[j]) // 인벤토리 하나당 먹은 아이템 비교해서
                    { // 인밴토리에 해당 아이템이 있는가?
                        exist = true;
                        break;
                    }
                }

                if (exist == false)
                { // 이 for문을 한번 실행 다하고도 false라는 말은 먹은 아이템이 없는아이템이란 말임
                    break;
                }
            }

            if (exist == false)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (Inventory[k] == null) // 리스트가 비어있으면
                    {
                        Inventory[k] = items[random]; // 추가
                        break;
                    }
                }
            }
        }
    }
    bool OBD = false;
    bool EBD = false;
    bool K = false;
    bool R = false;

    void Inven()
    { // 인벤토리창
        for (int k = 0; k < 3; k++)
        {
            if (Inventory[k] == "OneBlockDetector") // 한 블록 탐지기 먹었을시
            {
                OBD = true;
            }
            else if (Inventory[k] == "EightBlockDetector") {
                EBD = true;
            }
            else if (Inventory[k] == "Knight")
            {
                K = true;
            }
            else if (Inventory[k] == "RandomTeleport")
            {
                R = true;
            }
            /*
            items[0] = "OneBlockDetector"; // 한 블록 탐지기
            items[1] = "EightBlockDetector"; // 8방위 탐지기 
            items[2] = "Knight"; // 나이트
            items[3] = "RandomTeleport"; // 랜덤으로 아무대나 보냄
    */
        }
    }

    /* 아이템 먹으면 해당 bool 값이 true 됨 = 활성화
     활성화 된 아이템은 스크립트 작동함*/

    void OBDScript() {
        if (OBD == true) {
            // 8 방위 내 한 블록 클릑하면 해당 오브젝트가 보이게끔 해야됨

        }

    }
    void EBScript()
    {
        if (EBD == true)
        {
            // 사용 즉시 8방위의 구멍이 보여야함
        }

    }
    void KnightScript()
    {
        if (K == true)
        {
            // 채스의 나이트, 무브 포인트 다른 색깔로 위에 만들어서 클릭시 거기로 이동하게 예외는 똑같이
        }

    }
    void RTScript()
    {
        if (R == true)
        {
            // 플레이어 위치 랜덤으로 (맵 범위 안에서 감시탑이 아니고 시작과 도착 위치가 아닌 위치로 이동)
        }

    }
}
