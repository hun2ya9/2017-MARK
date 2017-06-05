using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
/* 플레이어가 구멍과 아이템이랑 만났을때 처리 및 라이프 관리*/
{
    public int maxLife = 3;
    public int life = 3; // 라이프 3개
    bool gameOver = false;

    public LayerMask LightHouse;
    bool Lpoint;
    public GameObject Trace; // 지나온 길
    public GameObject[] TracePoint = new GameObject[200]; // 지나온길 담을거임
    public static int t = 0; // 지나온 길 배열수 -> 이걸 또 따로 값을 저장하면 그게 점수내는데 쓰일거임
    public GameObject MovePointer;
    public Transform playerPoint; // 플레이어 위치
    public GameObject[] newMovePoint = new GameObject[9];
    Vector3 MPpoisition; // 무브포인트의 위치
    Text PlayerRoundHoles; // 플레이어 중심 8방위 구멍 수
    public LayerMask layerMask;
    public GameObject g;
    Hole f;

    public GameObject object_manager;
    ObjectManager_ script;
    GameObject lifeObj1;
    GameObject lifeObj2;
    GameObject lifeObj3;

    static int q = 0;

    /* getT()에 대한 설명
     만들어진 블록 프리팹이 안지워지는 경우가 발생하는걸 발견했다.
     알고보니 지나온길을 MovePoint 배열에 같이 넣어서 그랬다.
     MovePoint 배열은 계속 썻다 지웠다 반복하는데 값이 유지되어야 하는 지나온 길을 거기다 같이 써놨으니 당연히 오류가 날법도 했다.
     따라서 지나온 길을 TracePoint 배열에다가 따로 저장해놓고 static 정수 값 t를 만들때마다 1씩 더해서 그다음 배열에 저장하는 식으로 해버렸다.
     분명 더 좋은 방법이 있을건데 ...*/
    public int getT() { //  다른 클래스에서 static변수값을 끌어다 쓰는 메소드
        return t-1; // t-1인 이유는 지나온길 만들면서 t++ 하기 때문이다. (flag 에서 지워야 하는 값은 t++값이 아니고 t값이다.)
    }
    BFS script_bfs;
    
    void Start()
    {

        //startTime = Time.time;
        life = maxLife;
        script = object_manager.GetComponent<ObjectManager_>();

        script_bfs = GameObject.Find("Player").GetComponent<BFS>();

        if (script_bfs.is_root_interface() == 1)
            Debug.Log("exist");
        else
            Debug.Log("No_exist");
        print(script.getBFS_v());
        script.print_distance();
        //startTime = Time.time;
        life = maxLife;




        PlayerRoundHoles = GameObject.FindGameObjectWithTag("PlayerRoundHoles").GetComponent<Text>(); // 우측 상단 플레이어 중심 구멍수 택스트

        playerPoint.position = new Vector2(-(script.gridWorldSize.x * 5) + 5, (script.gridWorldSize.y * 5) - 5); // 플레이어 시작 위치 지정
        movePoint(); // 플레이어 시작 위치에 타일 까는작업
        StartCoroutine(CoMove()); //코루틴 시작 = update와 거의 동일하다 보면됨(마우스 클릭 위치 실시간으로 받게됨)
        f = g.GetComponent<Hole>();
        lifeObj1 = GameObject.Find("Life1"); // life 오브젝트
        lifeObj2 = GameObject.Find("Life2");
        lifeObj3 = GameObject.Find("Life3");


        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
    
    }

    // Update is called once per frame
    void Update()
    {

        script = object_manager.GetComponent<ObjectManager_>();
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);

        for (int i = 0; i < mapsize; i++) // 마우스 클릭한 블록에 구멍이 있을때 구멍 위치 -1로 해서 보이게함
        {
            if (f.holes[i].transform.position.x == playerPoint.position.x && f.holes[i].transform.position.y == playerPoint.position.y)
            {
                Transform ht1 = f.holes[i].GetComponent<Transform>();
                ht1.transform.position = new Vector3(f.holes[i].transform.position.x, f.holes[i].transform.position.y, -1);
                //Debug.Log(ht1.transform.position);
            }
        }

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
        if (Player.gameObject.tag == "Hole")
        {
            life -= 1; // 라이프 까짐
            //라이프 오브젝트 비활성화
            if (life == 2)
            {
                lifeObj3.SetActive(false);
            }
            else if (life == 1) {
                lifeObj2.SetActive(false);
            }
            else if (life == 0)
            {
                lifeObj1.SetActive(false);
            }
        }
    }


    public IEnumerator CoMove() //move 메소드를 update대신 코루틴을 사용해서 무한 반복시킨다.
    {
        while (this.enabled == true) // 아이템 사용시에는 무브포인트가 잠시 비활성화 되야해서 이렇게했음
        {
            StartCoroutine(move());
           yield return StartCoroutine(move());
            q = 0;
            StopCoroutine(move());
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

                /* 어이없는 부분에서 실수가 있었다.
                 * 매번 MovePoint 배열에 새로 위치가 할당되고 지워지고 반복하는데 왜 지나온 길을 MovePoint 배열에 같이 썻지??
                 * 해결책 : TracePoint라는 새로운 오브젝트 배열에 집어넣었다. 어차피 나중에 점수 계산에 끌어다 쓰면 될듯.
                 */
                if (i == 2 && j == 2 && q==0)
                { // 자기자신의 위치에는 지나온길 표시
                    TracePoint[t] = Instantiate(Trace); // 생성
                    TracePoint[t].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                    t++; // 한 스테이지 안에서 유지되는 값 지나온 길 개수은 아무리 많이 만들어도 맵 크기 넘게는 못만듬
                    q++;
                    Debug.Log("지나온 길 : " + t);
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

                if (countL == 0) // 간단하게 줄여보았음 굳이 1,2,3,4사분면 안 나눠도 이렇게 해도 ㄱㅊ
                {
                    if (MPpoisition.x > over.x || MPpoisition.x < -over.x || MPpoisition.y > over.y || MPpoisition.y < -over.y)
                    {
                        continue; // 맵 벗어나면 만들지 않는다.
                    }
                    else // 여기까지 왔으면 맵 안이라는 소리임
                    {
                        newMovePoint[i * 3 - j] = Instantiate(MovePointer); // 생성
                        newMovePoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                    }
                }

                // 플레이어 중심 8방위에 구멍 수 출력
                PlayerRoundHoles.text = view + "";

            }
        }
    }

    /* 매끄럽게 움직이는 방법 설계
     한프레임 :  0.015초~0.018초 평균 0.016정도 인데 고정값이 아니라서 변동이 심하다. => 0.2로 고정
     Lerp 는 a에서 b까지 가는에 t시간 걸렸을때의 위치다.

     출발지 a : 플레이어 위치
     도착지 b : 레이케스트에 히트된 물체의 위치
     시간 t : 시/거리 = 0.5f/Vector2.Distance(playerPoint.position, HitPos)
     단위 거리당 걸리는 시간  예를들어 0.5초 / 50m 이면 1% 당 0.01초걸린다~ 이런소리 => 목적지까지 1초 걸리면 도착

      여기서는  0.2f/Vector2.Distance(playerPoint.position, HitPos) : 목적지 까지 가는데 직선이든 대각선이든
      단위 거리당 0.2초로 나눔 : 직선은 10 만큼의 거리인데 1% 당 0.02 초 걸림 => 목적지 까지 가려면 1초를 만들어야됨
      yield return null 은 한 프레임 쉰다는 뜻
      50회 반복하면
      0.02 초 동안 단위거리 이동
      한 프레임 쉬고
      0.02 초 동안 단위거리 이동
      한 프레임 쉬고
      1초가 되었을때 도착지점까지 이동완료
      

     한 움직임을 50등분해서 1프레임당 10%씩 움직이면 10프레임 시 목적지 도착할 것이다.
     move 메소드 안에서 마우스 클릭시 플레이어 위치를 10번을 반복시킨다.*/

    IEnumerator move() // 최초 무브포인트 생성 => 클릭해서 좌표 저장 => 레이케스트 => 새 좌표로 이동 => 무브포인트 제거 => 바뀐 좌표에 대한 무브포인트 생성
    {
        if (Input.GetMouseButtonDown(0))
        { // 클릭시 좌표 저장 position을 화면 공간에서 월드 공간으로 변경시킵니다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인 카메라에서 마우스 위치(클릭한곳)로 광선발사 
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
            if (hit == false)
            {
                // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MovePoint")) // 무브포인터에서만 이동가능
            {

                /* 플레이어 위치에 히트된 물체의 위치를 대입해서 그쪽으로 이동하도록 간단하게 바꿨다.
                 * 하지만 한단계 더 나아가서 미끄러지듯이 스르륵 움직이도록 한번 해봅시다. */
                //Vector2 velocity = Vector2.one;
                //Vector2 sd = Vector2.SmoothDamp(playerPoint.position, hit.collider.gameObject.transform.position, ref velocity, 0.005f, Mathf.Infinity, Time.deltaTime);

                //playerPoint.position = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y);
                Vector3 HitPos = hit.collider.gameObject.transform.position;
                for (int z = 0; z < 50; z++)
                {
                 playerPoint.position = Vector2.Lerp(playerPoint.position, HitPos, 0.2f/Vector2.Distance(playerPoint.position, HitPos));
                    yield return null; // 한프레임 정지

                }
                for (int i = 0; i < 9; i++)
                {
                        DestroyObject(newMovePoint[i]);
                    }
                movePoint(); // 새 위치 할당
            }
        }
    }
}