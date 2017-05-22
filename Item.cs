using System.Collections.Generic; // 이걸 써야 List 사용가능
using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    //List<string> items = new List<string>(); // 아이템 종류 리스트
    string[] items = new string[4];
    //List<string> Inventory = new List<string>(); // 인밴토리 리스트
    string[] Inventory = new string[4];
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
     아이템 종류 리스트에 저장
     플레이어가 아이템과 충돌시 랜덤 숫자 Random.Range(0,4) 으로 아이템 종류 리스트에서 아무거나 가져와서
     해당 아이템이 존재하는가?
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

            for (int j = 0; j < 4; j++) // 랜덤으로 먹은 아이템이
            {
                if (items[random] == Inventory[j])
                { // 인밴토리에 해당 아이템이 있는가?
                    exist = true;
                }
                else
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (Inventory[k] == null) // 리스트가 비어있으면
                        {
                            Inventory[k] = items[random]; // 추가
                            break; // 추가하면 나가는거 해야되는데 너무 피곤하다 낼해야지....
                        }
                    }

                }
            }

        }
        Debug.Log(items[0]);
        Debug.Log(Inventory[0]);
        Debug.Log(Inventory[1]);

    }
}


    



