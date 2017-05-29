using System.Collections.Generic; // 이걸 써야 List 사용가능
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
   아이템 랜덤 배치, 겹쳐도 상관없는데 안보이게 할꺼고
    충돌처리해서 해당 위치가면 collider 발동해서 하단의 아이템창에 추가하는식으로
    아이템 종류는 랜덤인데 아이템 창에 없는 아이템 뜨도록 => 배열에 0번부터 확인
*/
public class Item : MonoBehaviour
{
    string[] items = new string[4];
    string[] Inventory = new string[3];
    public LayerMask layerMask1; // OneBlockDetector
    public LayerMask layerMask2; // Knight

    static bool OBD = false;
    static bool EBD = false;
    static bool K = false;
    static bool R = false;

    public Transform item; // 아이템 위치
    public Transform playerPoint; // 플레이어 위치
    public Transform ViewOneBlockPoint; // 마우스 클릭 위치를 저장하는 변수임 딱히 의미없음

    public GameObject object_manager;
    ObjectManager_ script;
    Player test;

    Vector3 MPpoisition; // 이 위치를 newOneBlockPoint에다 넘겨주는 역활
    public GameObject OneBlockPoint; // 고물 탐지기 실행시 바닥에 깔리는 타일
    GameObject[] newOneBlockPoint = new GameObject[9]; // 실제 오브젝트(OneBlockPoint)를 담는배열
    GameObject[] newKnightPoint = new GameObject[9]; // 실제 오브젝트(OneBlockPoint)를 담는배열
    public GameObject KnightPoint; // 고물 탐지기 실행시 바닥에 깔리는 타일
    Vector3 Kposition; // 이 위치를 KnightPoint에다 넘겨주는 역활
    Vector3 ReKposition;
    public GameObject g;
    Hole h;

    public static int useItem = 0; // 아이템 사용 횟수


     Button b1, b2, b3; // 버튼


    void Start()
    {
        b1 = GameObject.Find("Item_Button1").GetComponent<Button>();
        b2 = GameObject.FindGameObjectWithTag("ItemMenu").GetComponent<Button>();
        b3 = GameObject.Find("Item_Button3").GetComponent<Button>();


        test = GameObject.Find("Player").GetComponent<Player>();
        MakeItem();
    }

    void Update()
    {
    }

    public void MakeItem() {
        script = object_manager.GetComponent<ObjectManager_>();
        h = g.GetComponent<Hole>();

        Vector3 itemPosition;

        int i = 0, x = 0, y = 0;
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        int house_cnt = (mapsize / 2) + 1;   //맵 한 줄의 길이/2==설치할 아이템의 수 

        do
        {
            x = Random.Range(0, mapsize - 1);
            y = Random.Range(0, mapsize - 1);

            for (int k = 0; k < mapsize; k++)
            {
                for (int g = 0; g < mapsize; g++)
                {
                    itemPosition = new Vector2(-(script.gridWorldSize.x * 5) + 5 + 10 * x, (script.gridWorldSize.y * 5) - 5 - 10 * y);
                    if (itemPosition == script.grid[k, g].worldPosition) // 랜덤위치와 첫번째부터(0,0)~(끝,끝) 위치 비교한다.
                    {
                        if ((script.grid[k, g].is_trap == false) && (script.grid[k, g].is_lighthouse == false))  //트랩 또는 등대가 이미 설치되있냐
                        {
                            int valid = 1;  //등대를 설치해도되는 위치냐
                                            //nqueen problem으로 해보기!!!그러나 똑같은 배치만 나올 수도 있다, 리커젼 풀리는 순서가 같기때문
                            if (((x == 0) && (y == 0)) || ((x == mapsize - 1) && (y == mapsize - 1)))
                                valid = 0;

                            if (valid == 1)
                            {
                                Transform newhouse = Instantiate(item);
                                newhouse.position = new Vector3(script.grid[k, g].worldPosition.x, script.grid[k, g].worldPosition.y);
                                i++;
                            }
                        }
                    }
                }
            }
        } while (i != house_cnt);
    }


    /*아이템 먹는거 설계
     아이템 종류 배열에 저장
     플레이어가 아이템과 충돌시 랜덤 숫자 Random.Range(0,4) 으로 아이템 종류 배열에서 아무거나 가져와서
     해당 아이템이 존재하는가? 존재하면 다시값 할당받아서 없는 아이템 먹을때까지 반복
         */

    public Item()
    {
        items[0] = "OneBlockDetector"; // 한 블록 탐지기
        items[1] = "EightBlockDetector"; // 8방위 탐지기 
        items[2] = "Knight"; // 나이트
        items[3] = "RandomTeleport"; // 랜덤으로 아무대나 보냄
    }

    void OnTriggerEnter2D(Collider2D coll) // player가 item에 부딪힐때
    {
        bool exist = false; // 인벤토리에 먹은 아이템 있는지확인 = 기본값 없는걸로(false)
        int random;
        if (coll.gameObject.tag == "Item") // 부딪힌게 아이템 일때
        {
            random = Random.Range(0, 4); // 아이템 랜덤으로 먹어짐

            do
            {
                for (int j = 0; j < 3; j++) // 랜덤으로 먹은 아이템이
                {
                    if (items[random] == Inventory[j]) // 인벤토리 하나당 먹은 아이템 비교해서
                    { // 인밴토리에 해당 아이템이 있는가?
                        exist = true; // 있으면 더 검사할 필요도 없이 탈출
                        break;
                    } // else 값은 false 그대로
                }

                if (exist == true) // 아이템 존재할시
                {
                    random = Random.Range(0, 4); // 다시 할당
                    exist = false;
                }
                else {
                    break; // 아이템이 인벤토리에 없으니까 다음 단계로 넘어간다.
                }
            } while (true); // 인벤토리에 없는 아이템 먹을때까지 반복

            if (exist == false)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (Inventory[k] == null) // 리스트가 비어있으면
                    {
                        Inventory[k] = items[random]; // 추가
                        Debug.Log(Inventory[k] + "를 추가합니다.");
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

        //  ColorBlock chColor1 = b1.GetComponent<Button>().colors;
        // ColorBlock chColor2 = b2.GetComponent<Button>().colors;
        // ColorBlock chColor3 = b3.GetComponent<Button>().colors;
        Debug.Log(b1); Debug.Log(b2);

        Debug.Log(b1.colors);
        Debug.Log(b2.colors);

        ColorBlock chColor1 = b1.colors;
        ColorBlock chColor2 = b2.colors;
        ColorBlock chColor3 = b3.colors;
        Debug.Log(chColor1);
        Debug.Log(chColor2);

        // 버튼 활성화 상태에서 색 바꾸고싶은데 도저히 안되네

        for (int k = 0; k < 3; k++)
        {
            if (Inventory[k] == "OneBlockDetector") // 한 블록 탐지기 먹었을시
            {
                OBD = true;
                chColor1.normalColor = Color.red;
                Debug.Log("고물탐지기를 획득." + OBD);
            }
            else if (Inventory[k] == "EightBlockDetector") {
                EBD = true;
                chColor2.normalColor = Color.red;
                Debug.Log("탐지기를 획득." + EBD);
            }
            else if (Inventory[k] == "Knight")
            {
                chColor3.normalColor = Color.red;
                K = true;
                Debug.Log("나이트를 획득." + K);
            }
            else if (Inventory[k] == "RandomTeleport")
            {
                R = true;
                Debug.Log("함정에 빠졌습니다. 무작위 위치로 이동합니다." + R);
            }
        }
    }

    /* 아이템 먹으면 해당 bool 값이 true 됨 = 활성화
     활성화 된 아이템은 스크립트 작동함*/


    public void OBD_MakeBlock() // 고물 탐지기 사용시 타일 까는 메소드
    {
        // 1. 맵 범위 안에서 2. 현재 플레이어 위치 중심 3. 8방위 조건만족하면 타일 생성하도록, 등대에는 생성되면 안됨!
        script = object_manager.GetComponent<ObjectManager_>();

        // 무브포인트의 위치 = 타일 생성하는 곳임
        int n = 10;
        Vector2 over = new Vector2(script.gridWorldSize.x * 5, script.gridWorldSize.y * 5); // 맵 사이즈 저장
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);


        for (int i = 1; i < 4; i++) // 123   369 - 123  = 210, 543, 876 하면 배열 0부터 8번까지 저장
        { // 8방위
            for (int j = 1; j < 4; j++) // 123
            {
                int countL = 0;

                MPpoisition = new Vector2(playerPoint.position.x + n * (i - 2), playerPoint.position.y + n * (j - 2)); // -1, 0, 1
                                                                                                                       // 타일 깔 위치는 for문 돌릴때마다 위치 바껴야해서

                if (i == 2 && j == 2) // 중심에는 만들 필요 없음
                {
                    continue;
                }

                for (int x = 0; x < mapsize; x++)
                { // 무브 포인트와 등대가 부딪히면 예외카운트 + 이말은 생성 안한다는 말임
                    for (int y = 0; y < mapsize; y++)
                    {
                        if (script.grid[x, y].worldPosition == MPpoisition) // 무브포인트와 일치할때
                        {
                            if (script.grid[x, y].is_lighthouse == true) // 해당 위치가 등대일때 생성안함
                            {
                                countL++;
                            }
                        }
                    }
                }

                if (countL == 0)
                {
                    if (MPpoisition.x >= 0 && MPpoisition.y >= 0) // 1사분면
                    {
                        if (MPpoisition.x > over.x || MPpoisition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                            newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

                        }
                    }
                    else if (MPpoisition.x >= 0 && MPpoisition.y <= 0) // 4사분면
                    {
                        if (MPpoisition.x > over.x || MPpoisition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                            newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

                        }

                    }
                    else if (MPpoisition.x <= 0 && MPpoisition.y >= 0) // 2사분면
                    {
                        if (MPpoisition.x < -over.x || MPpoisition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                            newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }

                    }
                    else if (MPpoisition.x <= 0 && MPpoisition.y <= 0) //3사분면
                    {
                        if (MPpoisition.x < -over.x || MPpoisition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                            newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }
                    }
                }
            }
        }
    }

    int c = 0; // 한번만 반복 = 클릭 한번만 하면 됨
    public IEnumerator OBD_MouseClick() // 고물 탐지기 타일만 클릭 가능하고 클릭시 해당위치에 구멍이 있다면 구멍 보이게해줌 
    { // 원리는 MovePoint 깔고나서 클릭하는거랑 똑같음 RayCast쏴서 해당 위치의 Layer가 OneBlockDetector이면 위치 반환
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);

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

                    for (int i = 0; i < 9; i++)
                    {
                        DestroyObject(newOneBlockPoint[i]); // 기존 타일 위치 제거
                    }


                    Debug.Log("4단계");
                    for (int i = 0; i < mapsize; i++) // 마우스 클릭한 블록에 구멍이 있을때 구멍 위치 -1로 해서 보이게함
                    {
                        if (h.holes[i].transform.position.x == ViewOneBlockPoint.position.x && h.holes[i].transform.position.y == ViewOneBlockPoint.position.y)
                        {
                            Transform ht1 = h.holes[i].GetComponent<Transform>();
                            ht1.transform.position = new Vector3(h.holes[i].transform.position.x, h.holes[i].transform.position.y, -1);
                        }
                    }
                    c++; // 카운트 하나 올라가면서 반복문을 빠져나오게된다.
                }
            }
            yield return null; // while문이 돌기전에 여기서 잠시 대기를 하기때문에 먹통이 안되는듯함. 대기시간은 update랑 비슷하다고 봤었음
        }
    }


    /*코루틴 설명
     이 함수 안에서 여러 함수를 같이 쓰거나 아니면 무한 반복같은 작업을 하는데
     작동 순서를 정해주려고 사용

        이 아이템은 마우스로 블록 하나를 누르면 그 위치에 임의의 가상 블록(ViewOneBlockPoint)을 생성하고
        가상 블록이랑 실제 구멍오브젝트(holes[0~mapsize])와 위치 비교하는 방식인데
        아이템 사용중에는 반드시 MovePoint를 비활성화 시키고 해야됨 (둘다 마우스클릭을 받기 때문임!!)
        
        1.아이템 버튼을 누른다
        2.ItemButton 스크립트의 OBDuse 메소드를 가져온다
        3.OBDuse 메소드 안의  StartCoroutine(i.OBDScript())불러오는데 코루틴을 실행한다는 뜻
        4.OBDScript는 제일먼저 인벤토리의 값을 가져와서 해당 아이템이 사용 가능한지 확인한다
        5.사용 가능하다면 Player 오브젝트의 Player 스크립트 중단시킴
        6.ItemPoint 메소드를 불러와서 바닥에 타일을깐다.
        7. StartCoroutine(OneBlockDetectorMove()) 마우스 클릭 위치를 처리하는 코루틴 실행
        
        여기서 왜 이걸 코루틴으로 했냐면
        마우스클릭 할때까지 무한 반복이 되서 프로그램이 먹통이 되버린다. (보통 같으면 update 이런데 넣어야됨)
        마우스 클릭 받는다 - 클릭한 위치가 타일인지 확인하고 클릭한 블록의 중심 위치에 가상의 오브젝트 생성
        - 생성한 타일 삭제 -가상의 오브젝트와 구멍의 위치가 일치하는지 비교 후 일치하다면 z축 이동해서 눈에 보이게해줌
        - 카운트값 1 올라가면서 반복문 빠져나옴
        이 메소드의 마지막에 yield return null 이 update와 비슷한 기능을 하는것임

        이 메소드를 나오자마자
        yield return StartCoroutine(OneBlockDetectorMove());를 만나는데
        OneBlockDetectorMove()가 실행 끝날때까지 더이상 진행하지 말고 기다리고 있거라 라는 말임
        이것을 굳이 이 위치에 넣은 이유는 이 메소드가 도는 동안에는 MovePoint가 비활성되어야 되기 때문이다.
        
        그다음에 아이템 사용한것으로 처리하고 해당Inventory 배열 null로 초기화하고나서
        다시 Player 스크립트 활성화 시켜서
        MovePoint만들도록 해줬음
        끗
   


         */
    public IEnumerator OBDScript()
    {
        Inven(); // 버튼을 누르면 인벤토리를 불러와서 값을 확인한다.
        Debug.Log(OBD + "사용가능");
        if (OBD == true) // 아이템을 먹은 경우에만 실행해야됨
        {
            test.enabled = false; // 무브 포인트를 중단시킨다. StopCoroutine 해도 되었을듯??
            OBD_MakeBlock(); // 타일 까는 작업
            StartCoroutine(OBD_MouseClick()); // 타일 깔았으니 이제 타일을 마우스 클릭해야됨
            yield return StartCoroutine(OBD_MouseClick()); // 마우스 클릭할때까지 코드 진행 멈춤

            Debug.Log("고물탐지기 사용완료1" + OBD);
            OBD = false; // 다시 아이템을 없는 상태로 만들고
            useItem++; // 아이템 사용 횟수 +1
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "OneBlockDetector") // 한 블록 탐지기 제거
                {
                    Inventory[k] = null; // 배열 초기화
                    Debug.Log("고물탐지기 사용완료2" + OBD);
                }
            }

            yield return null;

            test.enabled = true; // 다시 true값 넣어줘야 무브포인트가 작동을함
            StartCoroutine(test.CoMove()); //무브포인트 코루틴 시작
        }
    }










    public void K_MakeBlock()
    {
        script = object_manager.GetComponent<ObjectManager_>();
        int c = 0;
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        // 1. 맵 범위 안에서 2. 현재 플레이어 위치 중심 3. 8방위 조건만족하면 타일 생성하도록, 등대에는 생성되면 안됨!
        // 무브포인트의 위치 = 타일 생성하는 곳임
        int n=10;
        for (int i = 1; i < 3; i++) // 12
        {
            for (int j = 1; j < 6; j++) // 12345
            {
                if (i == 1 && (j == 1 ||j == 3 || j == 5)) {
                    continue;
                }
                if (i == 2 && (j == 2 || j == 3 || j == 4)) {
                    continue;
                }

                int countL1 = 0; int countL2 = 0;

                Kposition = new Vector2(playerPoint.position.x + n * (i - 3), playerPoint.position.y + n * (j - 3));
                // -20, -10또는10 / -10, -20또는20
                ReKposition = new Vector2(playerPoint.position.x - n * (i - 3), playerPoint.position.y - n * (j - 3));
                Vector2 over = new Vector2(script.gridWorldSize.x * 5, script.gridWorldSize.y * 5); // 맵 사이즈 저장

                for (int x = 0; x < mapsize; x++)
                { // 무브 포인트와 등대가 부딪히면 예외카운트 + 이말은 생성 안한다는 말임
                    for (int y = 0; y < mapsize; y++)
                    {
                        if (script.grid[x, y].worldPosition == Kposition) // 무브포인트와 일치할때
                        {
                            if (script.grid[x, y].is_lighthouse == true) // 해당 위치가 등대일때 생성안함
                            {
                                countL1++;
                                Debug.Log("누락1");
                            }
                        }else if (script.grid[x, y].worldPosition == ReKposition) // 무브포인트와 일치할때
                        {
                            if (script.grid[x, y].is_lighthouse == true) // 해당 위치가 등대일때 생성안함
                            {
                                countL2++;
                                Debug.Log("누락2");
                            }
                        }
                    }
                }
                Debug.Log("ReKposition"+ReKposition);
                Debug.Log("Kposition" + Kposition);

                if (countL1 == 0)
                {
                    if (Kposition.x >= 0 && Kposition.y >= 0) // 1사분면
                    {
                        if (Kposition.x > over.x || Kposition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(Kposition.x, Kposition.y);
                            c++;
                        }
                    }
                    else if (Kposition.x >= 0 && Kposition.y <= 0) // 4사분면
                    {
                        if (Kposition.x > over.x || Kposition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(Kposition.x, Kposition.y);
                            c++;
                        }
                    }
                    else if (Kposition.x <= 0 && Kposition.y >= 0) // 2사분면
                    {
                        if (Kposition.x < -over.x || Kposition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(Kposition.x, Kposition.y);
                            c++;
                        }
                    }
                    else if (Kposition.x <= 0 && Kposition.y <= 0) //3사분면
                    {
                        if (Kposition.x < -over.x || Kposition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(Kposition.x, Kposition.y);
                            c++;
                        }
                    }
                }

                if (countL2 == 0) {

                    if (ReKposition.x >= 0 && ReKposition.y >= 0) // 1사분면
                    {
                        if (ReKposition.x > over.x || ReKposition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(ReKposition.x, ReKposition.y);
                            c++;
                        }
                    }
                    else if (ReKposition.x >= 0 && ReKposition.y <= 0) // 4사분면
                    {
                        if (ReKposition.x > over.x || ReKposition.y < -over.y)
                        {
                            Debug.Log("아마 여기서 누락");
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(ReKposition.x, ReKposition.y);
                            c++;
                        }
                    }
                    else if (ReKposition.x <= 0 && ReKposition.y >= 0) // 2사분면
                    {
                        if (ReKposition.x < -over.x || ReKposition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(ReKposition.x, ReKposition.y);
                            c++;
                        }
                    }
                    else if (ReKposition.x <= 0 && ReKposition.y <= 0) //3사분면
                    {
                        if (ReKposition.x < -over.x || ReKposition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newKnightPoint[c] = Instantiate(KnightPoint); // 생성
                            newKnightPoint[c].transform.position = new Vector3(ReKposition.x, ReKposition.y);
                            c++;
                        }
                        // 홀수맵에서는 좌표가 0일 수가 있음;; x,y가 하나이상 0인 경우임
                    }
                }
            }

        }
    }



    int c1 = 0; // 한번만 반복 = 클릭 한번만 하면 됨
    public IEnumerator K_MouseClick() // 고물 탐지기 타일만 클릭 가능하고 클릭시 해당위치에 구멍이 있다면 구멍 보이게해줌 
    { // 원리는 MovePoint 깔고나서 클릭하는거랑 똑같음 RayCast쏴서 해당 위치의 Layer가 OneBlockDetector이면 위치 반환
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);

        while (c1 == 0) // 클릭 할때까지 무한반복해야됨
        {
            if (Input.GetMouseButtonDown(0))
            { // 클릭시 좌표 저장

                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raycast.origin, Vector2.zero, Mathf.Infinity, layerMask2);
                Debug.Log(raycast);
                if (hit == false)
                {
                    Debug.Log("이게 나오면 안됩니다..");
                    // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Knight"))
                {

                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log(pos + "좌표");
                    /*범위 안에 들어왔을때 임의의 좌표(이거랑 구멍이랑 비교할거임)가 좌표가 칸의 중심으로 이동하게끔 하는거임
                     모든 오브젝트가 칸의 중심이 기준이라서 그렇게 해야 서로 비교가 되니까*/

                    Vector3 a = new Vector3(playerPoint.position.x - pos.x, playerPoint.position.y - pos.y); // a에 위치 저장
                    Debug.Log(a);
                    if ((a.x > -25 && a.x < -15) && (a.y > -15 && a.y < -5))
                    { // 우상
                        playerPoint.transform.position = new Vector2(playerPoint.position.x + 20, playerPoint.position.y + 10);
                    }
                    else if ((a.x > -25 && a.x < -15) && (a.y > 5 && a.y < 15))
                    { // 우하
                        playerPoint.transform.position = new Vector2(playerPoint.position.x + 20, playerPoint.position.y - 10);

                    }
                    else if ((a.x > -15 && a.x < -5) && (a.y > -25 && a.y < -15))
                    { // 우
                        playerPoint.transform.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y +20);
                    }
                    else if ((a.x > -15 && a.x < -5) && (a.y > 15 && a.y < 25))
                    { // 위
                        playerPoint.transform.position = new Vector2(playerPoint.position.x +10, playerPoint.position.y - 20);
                    }
                    else if ((a.x > 15 && a.x < 25) && (a.y > 5 && a.y < 15))
                    {// 아래
                        playerPoint.transform.position = new Vector2(playerPoint.position.x -20, playerPoint.position.y - 10);
                    }
                    else if ((a.x > 15 && a.x < 25) && (a.y > -15 && a.y < -5))
                    { // 좌상 
                        playerPoint.transform.position = new Vector2(playerPoint.position.x - 20, playerPoint.position.y + 10);
                    }
                    else if ((a.x > 5 && a.x < 15) && (a.y > 15 && a.y < 25))
                    { // 왼
                        playerPoint.transform.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y -20);
                    }
                    else if ((a.x > 5 && a.x < 15) && (a.y > -25 && a.y < -15))
                    { //좌하
                       playerPoint.transform.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y + 20);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        DestroyObject(newKnightPoint[i]); // 기존 타일 위치 제거
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        DestroyObject(test.newMovePoint[i]); // 기존 위치 제거
                    }
                    test.movePoint(); // 새 위치 할당
                


                c1++; // 카운트 하나 올라가면서 반복문을 빠져나오게된다.
                }
            }
            yield return null; // while문이 돌기전에 여기서 잠시 대기를 하기때문에 먹통이 안되는듯함. 대기시간은 update랑 비슷하다고 봤었음
        }
    }









    public void EBScript()
    {
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        script = object_manager.GetComponent<ObjectManager_>();

        Vector3 ViewEightBlockPoint;
        if (EBD == true)
        {
            // 사용 즉시 8방위의 구멍이 보여야함
                for (int i = 1; i < 4; i++)
                { // 8방위
                    for (int j = 1; j < 4; j++)
                    {
                        ViewEightBlockPoint = new Vector2(playerPoint.position.x + 10 * (i - 2), playerPoint.position.y + 10 * (j - 2));
                        for (int z = 0; z < mapsize; z++) // 마우스 클릭한 블록에 구멍이 있을때 구멍 위치 -1로 해서 보이게함
                        {
                            if (h.holes[z].transform.position.x == ViewEightBlockPoint.x && h.holes[z].transform.position.y == ViewEightBlockPoint.y)
                            {
                                Transform ht1 = h.holes[z].GetComponent<Transform>();
                                ht1.transform.position = new Vector3(h.holes[z].transform.position.x, h.holes[z].transform.position.y, -1);
                            }
                        }
                    }
                }
            EBD = false; // 다시 아이템을 없는 상태로 만들고
            useItem++; // 아이템 사용 횟수 +1
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "EightBlockDetector") // 여덟 블록 탐지기 제거
                {
                    Inventory[k] = null; // 배열 초기화
                    Debug.Log("탐지기 사용완료2" + EBD);
                }
            }
        }
    }
    public IEnumerator KnightScript()
    {

        Inven(); // 버튼을 누르면 인벤토리를 불러와서 값을 확인한다.
        Debug.Log(K + "사용가능");
        if (K == true) // 아이템을 먹은 경우에만 실행해야됨
        {
              test.enabled = false; // 무브 포인트를 중단시킨다. StopCoroutine 해도 되었을듯??

            K_MakeBlock(); // 타일 까는 작업
            StartCoroutine(K_MouseClick()); // 타일 깔았으니 이제 타일을 마우스 클릭해야됨
            yield return StartCoroutine(K_MouseClick()); // 마우스 클릭할때까지 코드 진행 멈춤

            Debug.Log("나이트 사용완료1" + K);
            K = false; // 다시 아이템을 없는 상태로 만들고
            useItem++; // 아이템 사용 횟수 +1
            for (int k = 0; k < 3; k++) // 인벤토리 전체 검사해서 해당 아이템이 위치했던 배열을 null로 초기화
            {
                if (Inventory[k] == "Knight") // 나이트 제거
                {
                    Inventory[k] = null; // 배열 초기화
                    Debug.Log("나이트 사용완료2" + K);
                }
            }

            yield return null;

            test.enabled = true; // 다시 true값 넣어줘야 무브포인트가 작동을함
            StartCoroutine(test.CoMove()); //무브포인트 코루틴 시작
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
