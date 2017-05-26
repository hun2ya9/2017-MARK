using System.Collections.Generic; // 이걸 써야 List 사용가능
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item : MonoBehaviour
{
    string[] items = new string[4];
    string[] Inventory = new string[3];
   // public Transform playerPoint;
    static List<Vector3> LH = new List<Vector3>(); //등대 위치가 담긴 리스트
    static List<Vector3> H = new List<Vector3>(); //구멍 위치가 담긴 리스트
    List<Vector3> I = new List<Vector3>(); //아이템 위치가 담긴 리스트
    //public GameObject OneBlockPoint;
    //GameObject[] newOneBlockPoint = new GameObject[9];
   // Vector3 MPpoisition; // 무브포인트의 위치
    public LayerMask layerMask1; // OneBlockDetector
   // GameObject MovePointer;
   static bool OBD = false;
   static bool EBD = false;
    static bool K = false;
    static bool R = false;
    PlayerMove test;
    Hole holetest;
    public Transform playerPoint; // 플레이어 위치
    public Transform ViewOneBlockPoint; // 그냥 위치를 저장하는 변수임 딱히 의미없음

    /*
    아이템 랜덤 배치, 겹쳐도 상관없는데 안보이게 할꺼고
     충돌처리해서 해당 위치가면 collider 발동해서 하단의 아이템창에 추가하는식으로
     아이템 종류는 랜덤인데 아이템 창에 없는 아이템 뜨도록 => 배열에 0번부터 확인
 */
    void Start()
    {
       test = GameObject.Find("Player").GetComponent<PlayerMove>();   
        holetest = GameObject.Find("Trap").GetComponent<Hole>();

    }

    void Update()
    {
    }


    /* 게임을 실행하면 등대 주변에선 구멍이 보이고 나머지 위치에선 안 보이고 할건데 이거는 여러가지 방법중에 
     * 2D 물체를 Z축 건드려서 눈에 안보이게 하는 교묘한 방법을 썼다
     * 
     PlayerMove에서 레이케스트를 전체 맵에 쏴서 오브젝트의 위치 가져오는것 처럼 여기서도 똑같은 방법을 쓰려했지만
     레이케스트를 사용했을땐 2D라 Z축 값을 무시하기 때문에
     눈에 보이는 (z=0)인 구멍과  /  눈에 안보이는(z=1)구멍 모두 동일한 것으로 인식이 되었다.

     전체 맵에 레이케스트 쏴서 안보이는 구멍(z=1)의 위치를 따로 배열에 저장하고
     그 배열의 위치가 고물 탐지기의 블록(8방위 깔은거)과 일치하면 z축 0로 만들어서 보이게끔 하려했는데
     해보니까 레이케스트가 z = 0과 z=1 모두 인식해버렸다(구분이 안됨)는 말이다.
     
     그래서 어쩔 수 없이 Hole클래스에서 메소드로 가져오는 방법을 썼다.
     
         지금 확실해진 것은 
         다른 클래스에서 값을 가져오는 방법은 객체를 만들고 객체.변수를 가져올게 아니라
         메소드로 가져와야 */

    int c = 0; // 한번만 반복 = 클릭 한번만 하면 됨
    public IEnumerator OneBlockDetectorMove() // 고물 탐지기 타일만 클릭 가능하고 클릭시 해당위치에 구멍이 있다면 구멍 보이게해줌 
    { // 원리는 MovePoint 깔고나서 클릭하는거랑 똑같음 RayCast쏴서 해당 위치의 Layer가 OneBlockDetector이면 위치 반환
       
        while (c == 0) // 클릭 할때까지 무한반복해야됨
        {
            if (Input.GetMouseButtonDown(0))
            { // 클릭시 좌표 저장

                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raycast.origin, Vector2.zero, Mathf.Infinity, layerMask1);
                Debug.Log(raycast);
                if (hit == false)
                {
                    Debug.Log("이게 나오면 안됩니다..");
                    // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("OneBlockDetector")) // OneBlockDetector에서만 이동가능
                {// 클릭시 해당 블록의 구멍이 있다면 z축 +2로 보여야함.. 근데 클릭한 좌표가.
                   
                    ObjectManager o = new ObjectManager();
                    Vector2 gridWorldSize = o.getSize();
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log(pos + "좌표");
                    /*범위 안에 들어왔을때 임의의 좌표(이거랑 구멍이랑 비교할거임)가 좌표가 칸의 중심으로 이동하게끔 하는거임
                     모든 오브젝트가 칸의 중심이 기준이라서 그렇게 해야 서로 비교가 되니까*/
                   
                    Vector3 a = new Vector3(playerPoint.position.x - pos.x, playerPoint.position.y - pos.y); // a에 위치 저장

                    if ((a.x > -15 && a.x < -5) && (a.y > -15 && a.y < -5))
                    { // 우상
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y + 10);
                    }
                    else if ((a.x > -15 && a.x < -5) && (a.y > 5 && a.y < 15))
                    { // 우하
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y - 10);

                    }
                    else if ((a.x > -15 && a.x < -5) && (a.y > -5 && a.y < 5))
                    { // 우
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y);
                    }
                    else if ((a.x > -5 && a.x < 5) && (a.y > -15 && a.y < -5))
                    { // 위
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y + 10);
                    }
                    else if ((a.x > -5 && a.x < 5) && (a.y > 5 && a.y < 15))
                    {// 아래
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y - 10);
                    }
                    else if ((a.x > 5 && a.x < 15) && (a.y > -15 && a.y < -5))
                    { // 좌상 
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y + 10);
                    }
                    else if ((a.x > 5 && a.x < 15) && (a.y > -5 && a.y < 5))
                    { // 왼
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y);
                    }
                    else if ((a.x > 5 && a.x < 15) && (a.y > 5 && a.y < 15))
                    { //좌하
                        ViewOneBlockPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y - 10);
                    }
                    else
                    {
                    }
                    
                    /* PlayerMove와 다른점은 여기서부터다
                     왜 나머지를 여기서 안하고 굳이 Hole클래스에서 끌고 와야했냐면 구멍의 위치가 필요했기 때문.
                     */
                    holetest.OBD();
                    c++; // 카운트 하나 올라가면서 반복문을 빠져나오게된다.
                }

            }
            yield return null; //솔직히 이게 어떻게 작동하는지 모르겠고 이해도안됨. 다만 이 위치에 안 넣으면 먹통이 되어버림
        } // 코루틴 이해가 안된다... 반복과정에서 순차적 실행(A 끝날때까지 대기했다가 B실행 이런거) / 실행 도중 멈췄다가 다시 그 상태에서 실행 뭐 이런 느낌인데


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
                    } // else 값은 false 그대로
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
                        Debug.Log(Inventory[k]+"를 추가합니다.");
                        Inven();
                        break;
                    }
                }
            }
        }
        coll.gameObject.GetComponent<Collider2D>().enabled = false; // 아이템은 한번만 먹을 수 있게 충돌처리가 안되게끔 아에 꺼버림
    }


    void Inven()
    { // 인벤토리창
        for (int k = 0; k < 3; k++)
        {
            if (Inventory[k] == "OneBlockDetector") // 한 블록 탐지기 먹었을시
            {
                OBD = true;
                Debug.Log("고물탐지기를 획득."+OBD);
            }
            else if (Inventory[k] == "EightBlockDetector") {
                EBD = true;
                Debug.Log("탐지기를 획득." + EBD);
            }
            else if (Inventory[k] == "Knight")
            {
                K = true;
                Debug.Log("나이트를 획득." + K);
            }
            else if (Inventory[k] == "RandomTeleport")
            {
                R = true;
                Debug.Log("함정에 빠졌습니다. 무작위 위치로 이동합니다." + R);
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

    public void OBDUse() {
        StartCoroutine(OBDScript());
    }

    public IEnumerator OBDScript()
    {
        //holetest = GameObject.Find("Trap").GetComponent<Hole>();

        Inven(); // 버튼을 누르면 인벤토리를 불러와서 값을 확인한다.
        Debug.Log(OBD + "사용가능");
        if (OBD == true) // 아이템을 먹은 경우에만 실행해야됨
        {
            test.enabled = false; // 무브 포인트를 중단시킨다. StopCoroutine 해도 되었을듯??
            holetest.Z2(); // 타일 까는 작업
            StartCoroutine(OneBlockDetectorMove()); // 타일 깔았으니 이제 타일을 마우스 클릭해야됨
            yield return StartCoroutine(OneBlockDetectorMove()); // 마우스 클릭할때까지 코드 진행 멈춤

           // yield return new WaitForSeconds(1);
            //test.StartCoroutine(test.CoMove()); // 게임 오브젝트에 붙여놓은 스크립트 이름으로 객체로 만들어서 그 객체로 접근 
            Debug.Log("고물탐지기 사용완료1" + OBD);

            OBD = false; // 다시 아이템을 없는 상태로 만들고
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "OneBlockDetector") // 한 블록 탐지기 제거
                {
                    Inventory[k] = null; // 리스트 비운다
                    Debug.Log("고물탐지기 사용완료2" + OBD);
                }
            }
            
            yield return null;

            test.enabled = true; // 다시 true값 넣어줘야 무브포인트가 작동을함
            StartCoroutine(test.CoMove()); //무브포인트 코루틴 시작
        }
    }

    // yield return null;

    public void EBScript()
    {
        if (EBD == true)
        {
            // 사용 즉시 8방위의 구멍이 보여야함
            holetest.EBD();

            EBD = false; // 다시 아이템을 없는 상태로 만들고
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "EightBlockDetector") // 여덟 블록 탐지기 제거
                {
                    Inventory[k] = null; // 리스트 비운다
                    Debug.Log("탐지기 사용완료2" + EBD);
                }
            }
        }

    }
    public void KnightScript()
    {
        if (K == true)
        {
            // 채스의 나이트, 무브 포인트 다른 색깔로 위에 만들어서 클릭시 거기로 이동하게 예외는 똑같이

            K = false; // 다시 아이템을 없는 상태로 만들고
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "Knight") // 나이트 제거
                {
                    Inventory[k] = null; // 리스트 비운다
                    Debug.Log("탐지기 사용완료2" + K);
                }
            }
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
