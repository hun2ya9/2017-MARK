using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
/* 플레이어가 구멍과 아이템이랑 만났을때 처리 및 라이프 관리*/
{
    public int maxLife = 3;
    public static int life = 3; // 라이프 3개
    bool gameOver = false;
    List<Vector3> h = new List<Vector3>(); //구멍 위치가 담긴 리스트


    public LayerMask LightHouse;
    bool Lpoint;
    public GameObject Trace; // 지나온 길
    public GameObject MovePointer;
    public Transform playerPoint; // 플레이어 위치
    public GameObject[] newMovePoint = new GameObject[9];
    Vector3 MPpoisition; // 무브포인트의 위치
    Text PlayerRoundHoles;
    public LayerMask layerMask;
    public GameObject g;
    Hole f;

    public GameObject object_manager;
    ObjectManager_ script;

    // Use this for initialization
    void Start()
    {
        life = maxLife;
        script = object_manager.GetComponent<ObjectManager_>();
        
        PlayerRoundHoles = GameObject.FindGameObjectWithTag("PlayerRoundHoles").GetComponent<Text>();

        playerPoint.position = new Vector2(-(script.gridWorldSize.x * 5) + 5, (script.gridWorldSize.y * 5) - 5);
        movePoint();
        StartCoroutine(CoMove()); //코루틴 시작
        f = g.GetComponent<Hole>();


    }

    // Update is called once per frame
    void Update()
    {
        if (life == 0)
        {
            if (!gameOver)
            {
                Die();
            }
        }
        end();
    }
    void Die()
    { // 사망시
        gameOver = true;
        SceneManager.LoadScene("Game Over UI'");
    }
    void RestartStage()
    {
        // 이부분은 게임 매니저와 연결해서 점수 시간 보여주고 매뉴로 빠지도록해야됨
    }


    void end()
    {
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        if (playerPoint.position.x == script.grid[mapsize-1,mapsize-1].worldPosition.x && playerPoint.position.y == script.grid[mapsize - 1, mapsize - 1].worldPosition.y)
        {
            SceneManager.LoadScene("Stage Result UI");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D Player)
    {
        script = object_manager.GetComponent<ObjectManager_>();
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);

        if (Player.gameObject.tag == "Hole")
        {
            life -= 1; // 라이프 까짐

            for (int i = 0; i < mapsize; i++) // 마우스 클릭한 블록에 구멍이 있을때 구멍 위치 -1로 해서 보이게함
            {
                if (f.holes[i].transform.position.x == playerPoint.position.x && f.holes[i].transform.position.y == playerPoint.position.y)
                {
                    Transform ht1 = f.holes[i].GetComponent<Transform>();
                    ht1.transform.position = new Vector3(f.holes[i].transform.position.x, f.holes[i].transform.position.y, -1);
                }
            }

        Debug.Log(life);
        }
    }


    public IEnumerator CoMove() //move 메소드를 update대신 코루틴을 사용해서 무한 반복시킨다.
    {
        while (this.enabled == true) // 아이템 사용시에는 무브포인트가 잠시 비활성화 되야해서 이렇게했음
        {
            move();
            yield return null;
        }
    }


    public void movePoint()
    {
        script = object_manager.GetComponent<ObjectManager_>();

        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        // 1. 맵 범위 안에서 2. 현재 플레이어 위치 중심 3. 8방위 조건만족하면 타일 생성하도록, 등대에는 생성되면 안됨!
        // 무브포인트의 위치 = 타일 생성하는 곳임
        int n = 10;

        int view = 0; // 플레이어 중심 8방위 구멍 개수
        for (int i = 1; i < 4; i++) // 123   369 - 123  = 210, 543, 876 하면 배열 0부터 8번까지 저장
        { // 8방위
            for (int j = 1; j < 4; j++) // 123
            {
                int countL = 0;


                MPpoisition = new Vector2(playerPoint.position.x + n * (i - 2), playerPoint.position.y + n * (j - 2)); // -1, 0, 1
                Vector2 over = new Vector2(script.gridWorldSize.x * 5, script.gridWorldSize.y * 5); // 맵 사이즈 저장

                if (i == 2 && j == 2)
                { // 자기자신의 위치에는 지나온길 표시
                    newMovePoint[i * 3 - j] = Instantiate(Trace); // 생성
                    newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
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
                            else if (script.grid[x, y].is_trap == true)
                            { // 해당 위치가 구멍일때 생성안하면서 view값 +1
                                view++;

                            }
                        }
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
                    else if (MPpoisition.x == 0)
                    { // x가 0일 경우

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
                        else
                        { // 딱 중심일때
                            newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                            newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }

                    }
                    else if (MPpoisition.y == 0)
                    { // y가 0일 경우
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인 카메라에서 마우스 위치(클릭한곳)로 광선발사 
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
            if (hit == false)
            {
                // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MovePoint")) // 무브포인터에서만 이동가능
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);// 마우스 찍은 위치 좌표로 저장
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

                for (int i = 0; i < 9; i++)
                {
                    DestroyObject(newMovePoint[i]); // 기존 위치 제거
                }

                movePoint(); // 새 위치 할당
            }
        }

    }
}


         