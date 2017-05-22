using System.Collections;
using System.Collections.Generic; // 이걸 써야 List 사용가능
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{/*
    Vector3 gg;
    public PlayerMove() {
      // gg = playerPoint.position;
        gg = new Vector3(playerPoint.position.x, playerPoint.position.y);
    }
    */
    public LayerMask LightHouse;
    bool Lpoint;
    public GameObject Move;
    public GameObject MovePointer;
    // public Transform MovePoint; // 플레이어가 갈 수 있는 바닥
    public Transform playerPoint; // 플레이어 위치
    GameObject[] newMovePoint = new GameObject[9];
    Vector3 MPpoisition; // 무브포인트의 위치
    List<Vector3> LH = new List<Vector3>(); //등대 위치가 담긴 리스트
    List<Vector3> H = new List<Vector3>(); //구멍 위치가 담긴 리스트
    List<Vector3> I = new List<Vector3>(); //아이템 위치가 담긴 리스트
  
    Text PlayerRoundHoles;
    public LayerMask layerMask;
    void Start()
    {
        PlayerRoundHoles = GameObject.FindGameObjectWithTag("PlayerRoundHoles").GetComponent<Text>();
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        playerPoint.position = new Vector2(-(gridWorldSize.x * 5) + 5, (gridWorldSize.y * 5) - 5);

        movePoint();
    }
    void Update()
    {
        
        move(); // 플레이어의 위치를 계속 update 해줘야 하기 때문에 여기 들어감
    }
    
       



// 마우스로 이동 + 태그가 블록인곳만 갈 수 있고 감시탑에는 못가도록
void movePoint()
    {
        // 1. 맵 범위 안에서 2. 현재 플레이어 위치 중심 3. 8방위 조건만족하면 타일 생성하도록, 등대에는 생성되면 안됨!
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        // 무브포인트의 위치 = 타일 생성하는 곳임
        int n = 10;



        for (int x = 0; x < gridWorldSize.x; x++) // 레이케스트를 통해서 등대 위치 받아와서 리스트 추가하는 작업임
        {
            for (int y = 0; y < gridWorldSize.y; y++)
            {
                Vector3 point = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * x, (gridWorldSize.y * 5) - 5 - 10 * y);
                RaycastHit2D hit = Physics2D.Raycast(point, new Vector2(0,0), Mathf.Infinity);//(pos, Vector2.zero);

                if (hit == false)
                {
                    // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("LightHouse")) // 무브포인터에서만 이동가능
                { // collider 넣었기 때문에 등대 프리팹에도 Box 충돌체 추가해야지만 이게 작동한다.
                    LH.Add(point);
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hole"))
                {
                    H.Add(point);
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Item")) {

                    I.Add(point);
                }


            }
        }

        int view = 0; // 플레이어 중심 8방위 구멍 개수
        for (int i = 1; i < 4; i++) // 123   369 - 123  = 210, 543, 876 하면 배열 0부터 8번까지 저장
        { // 8방위
            for (int j = 1; j < 4; j++) // 123
            {
                int countL = 0;
               

                MPpoisition = new Vector3(playerPoint.position.x + n * (i - 2), playerPoint.position.y + n * (j - 2)); // -1, 0, 1
                Vector2 over = new Vector2(gridWorldSize.x * 5, gridWorldSize.y * 5); // 맵 사이즈 저장

                if (i == 2 && j == 2)
                { // 자기자신의 위치에는 지나온길 표시
                    newMovePoint[i * 3 - j] = Instantiate(Move); // 생성
                    newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                }


                for (int z = 0; z < LH.Count; z ++) { // 무브 포인트와 등대가 부딪히면 예외카운트 + 이말은 생성 안한다는 말임
                    if (LH[z] == MPpoisition)
                    {
                        countL++;
                    }
               }
                int count = 0; // 자꾸 중복 반복되서 그냥 1일때 멈추게끔하게
                for (int z = 0; z < Mathf.RoundToInt(gridWorldSize.x); z++) // 구멍 수만큼 반복하면 됨
                {
                    if (count == 1) {
                        break;
                    }
                    if (H[z] == MPpoisition) // 무브 포인트와 구멍위치 일치
                    {
                        view++;
                        count++;
                    }
                }
                


                if (countL == 0)
                {
                    if (MPpoisition.x > 0 && MPpoisition.y > 0) // 1사분면
                    {
                        if (MPpoisition.x > over.x || MPpoisition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

                        }
                    }
                    else if (MPpoisition.x > 0 && MPpoisition.y < 0) // 4사분면
                    {
                        if (MPpoisition.x > over.x || MPpoisition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

                        }

                    }
                    else if (MPpoisition.x < 0 && MPpoisition.y > 0) // 2사분면
                    {
                        if (MPpoisition.x < -over.x || MPpoisition.y > over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }

                    }
                    else if (MPpoisition.x < 0 && MPpoisition.y < 0) //3사분면
                    {
                        if (MPpoisition.x < -over.x || MPpoisition.y < -over.y)
                        {
                            continue; // 맵 벗어나면 만들지 않는다.
                        }
                        else // 여기까지 왔으면 맵 안이라는 소리임
                        {
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }
                        // 홀수맵에서는 좌표가 0일 수가 있음;; x,y가 하나이상 0인 경우임
                    }
                    else if(MPpoisition.x == 0){ // x가 0일 경우

                        if (MPpoisition.y > 0) // 위쪽
                        {
                            if (MPpoisition.y > over.y)
                            {
                                continue; // 맵 벗어나면 만들지 않는다.
                            }
                            else // 여기까지 왔으면 맵 안이라는 소리임
                            {
                                newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                                newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                        }
                        else if (MPpoisition.y < 0) // 아래쪽
                        {
                            if (MPpoisition.y < -over.y)
                            {
                                continue; // 맵 벗어나면 만들지 않는다.
                            }
                            else // 여기까지 왔으면 맵 안이라는 소리임
                            {
                                newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                                newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                        }
                        else { // 딱 중심일때
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }

                    } else if(MPpoisition.y == 0){ // y가 0일 경우
                        if (MPpoisition.x > 0) // 오른쪽
                        {
                            if (MPpoisition.x > over.x)
                            {
                                continue; // 맵 벗어나면 만들지 않는다.
                            }
                            else // 여기까지 왔으면 맵 안이라는 소리임
                            {
                                newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                                newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                        }
                        else if (MPpoisition.x < 0) // 아래쪽
                        {
                            if (MPpoisition.x < -over.x)
                            {
                                continue; // 맵 벗어나면 만들지 않는다.
                            }
                            else // 여기까지 왔으면 맵 안이라는 소리임
                            {
                                newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                                newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                        }
                        else
                        { // 딱 중심일때
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }
                    }
                        else
                        {
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }
                    }
            }
            }
        /* 플레이어 중심 8방위에 구멍 수 출력*/
        PlayerRoundHoles.text = view + "";
        
    }



    void move() // 최초 무브포인트 생성 => 클릭해서 좌표 저장 => 레이케스트 => 새 좌표로 이동 => 무브포인트 제거 => 바뀐 좌표에 대한 무브포인트 생성
    {
        if (Input.GetMouseButtonDown(0))
        { // 클릭시 좌표 저장
          /*position을 화면 공간에서 월드 공간으로 변경시킵니다.
  */
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ray.direction-
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask );//(pos, Vector2.zero);
                                                                                                        //if (hit.collider.tag == "MovePoint")
            if (hit == false) {
                // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
            } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MovePoint")) // 무브포인터에서만 이동가능
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                /*범위 안에 들어왔을때 해당 칸의 중심으로 이동하게끔
                오른쪽 : -15 < x < -5 , -5 < y < 5 v
                왼쪽   :  5 < x < 15 , -5 < y < 5
                위     : -5 < x < 5, -15 < y < -5         
                아래   : -5 < x < 5, 5 < y < 15        
                대각선 
                우하   : -15 < x < -5 , 5 < y < 15 v
                좌하   :  5 < x < 15  , 5 < y < 15
                우상   : -15 < x < -5 , -15 < y < -5 v
                좌상   : 5 < x < 15 , -15 < y < -5
                 * 
                 */
                Vector3 a = new Vector3(playerPoint.position.x - pos.x, playerPoint.position.y - pos.y); // a에 위치 저장
                
                if ((a.x > -15 && a.x < -5) && (a.y > -15 && a.y < -5))
                { // 우상
                    playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y + 10);
                }
                else if ((a.x > -15 && a.x < -5) && (a.y > 5 && a.y < 15))
                { // 우하
                    playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y - 10);
                }
                else if ((a.x > -15 && a.x < -5) && (a.y > -5 && a.y < 5))
                { // 우
                    playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y);
                }
                else if ((a.x > -5 && a.x < 5) && (a.y > -15 && a.y < -5))
                { // 위
                    playerPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y + 10);
                }
                else if ((a.x > -5 && a.x < 5) && (a.y > 5 && a.y < 15))
                {// 아래
                    playerPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y - 10);
                }
                else if ((a.x > 5 && a.x < 15) && (a.y > -15 && a.y < -5))
                { // 좌상 
                    playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y + 10);
                }
                else if ((a.x > 5 && a.x < 15) && (a.y > -5 && a.y < 5))
                { // 왼
                    playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y);
                }
                else if ((a.x > 5 && a.x < 15) && (a.y > 5 && a.y < 15))
                { //좌하
                    playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y - 10);
                }
                else
                {
                }
            }
            else {
                Debug.Log("다른곳을 눌렀따.");
            }

            for (int i = 0; i < 9; i++)
            {
                DestroyObject(newMovePoint[i]); // 기존 위치 제거
            }

            movePoint(); // 새 위치 할당
            }

    }
}