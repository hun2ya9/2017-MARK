using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour
{

    public GameObject flag; // 깃발
    GameObject[] flagPos = new GameObject[100]; // 플레그 실제 위치 저장할꺼임
    Player test;
    static int z = 0;
    public GameObject object_manager;
    ObjectManager_ script;
    /* 깃발 설계
     update에 flag() 올려놔서 언제든 우클릭 받으면 생성하도록 
     우클릭 - 무언가에 히트될때(맵전체에 BoxColider2D 생성 되어 있으므로 맵 안에서만 작동됨
     */
    IEnumerator makeFlag() // 깃발
    {
        script = object_manager.GetComponent<ObjectManager_>();

        while (true) // 아이템 사용시에는 무브포인트가 잠시 비활성화 되야해서 이렇게했음
        {
            if (Input.GetMouseButtonDown(1))
            {
                test.enabled = false; // 무브포인트 중단


                for (int i = 0; i < 9; i++)
                {
                    DestroyObject(test.newMovePoint[i]); // 기존 위치 제거 + 지나온 길도 지워야함
                    DestroyObject(test.TracePoint[Player.t]);
                    if (Player.t != 0) // 시작 지점에서 깃발 꼽을시엔 t=0이라 t-1 = -1 : 오류!!
                        Player.t -= 1; // 1빼줘라.. 안그럼 t값 무한대로 올라간다.

                }
                /* 이제 확실히 알았다.
                 정적 메소드나 정적 변수는 클래스.메소드 혹은 클래스.변수로 접근이 가능하다...
                 만약 정적이 아니라면 객체를 만들어서 객체 변수.메소드 이딴식으로 해야된다
                 정적 변수가 아니면 이런 방식으로 (객체 변수.변수) 접근 불가능 그래서 get매소드 쓰는거다..!!
                 */


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인 카메라에서 마우스 위치(클릭한곳)로 광선발사 
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                /* 굉장히 큰 문제점이 있다.
                 마우스 클릭시 타일과 충돌 할 경우 깃발과 충돌 할 경우 2가지가 있는데
                 그게 내 마음대로 클릭되는게 아니다!...
                 그냥 태그나 레이어로 구분지을 수 있는게 아님
                 깃발이 있는데도 타일을 누를수도 있어서 계속 버그가 발생한다.

                두번째로 깃발의 위치를 저장하려하니 이 또한 문제가 있다.
                그냥 다른거처럼 할까?

                 */
                if (hit == false)
                {
                    // 히트된 게 없을때 => 이걸 추가하는 이유는 맵 밖으로 안나가게 하려고
                }
                else { // 충돌시 위치비교 정말 비효율적이지만 맵 전체 반복 돌려서 해당 위치를 가져온다.
                    for (int i = 0; i < script.gridWorldSize.x; i++)
                    {
                        for (int j = 0; j < script.gridWorldSize.y; j++)
                        {
                            if (script.grid[i, j].worldPosition == hit.collider.gameObject.transform.position)
                            {
                                if (script.grid[i, j].is_lighthouse) // 해당 위치에 등대가 있네
                                {
                                    // 걍 넘어가샘
                                }
                                else if (script.grid[i, j].is_flag) // 이번엔 깃발이 있네
                                {
                                    //삭제
                                    for (int z = 0; z < 100; z++) // 실제 깃발의 위치와 비교
                                    {
                                        if (flagPos[z] != null && flagPos[z].transform.position == script.grid[i, j].worldPosition)
                                        {
                                            DestroyObject(flagPos[z]); //이야 없어지긴 하는데 완전히 없애버리는데ㅋㅋ?
                                        }

                                    }
                                }
                                else // 아 아무것도 없네
                                {
                                    script.grid[i, j].is_flag = true; // 이 위치에는 등대를 만든다.
                                    flagPos[z] = Instantiate(flag); // 여기에 실제 오브젝트가 저장이 됨.
                                    flagPos[z].transform.position = script.grid[i, j].worldPosition; // 오브젝트의 위치지정
                                    // 도저히 머리가 안돌아간다. 그냥 z값은 무한히 올라가게끔..어차피 몇 번 안쓸거임
                                    z++;
                                }
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(0.3f);
                test.movePoint(); // 새 위치 할당
                test.enabled = true; // 다시 true값 넣어줘야 무브포인트가 작동을함
                StartCoroutine(test.CoMove()); //무브포인트 코루틴 중단시킴 => 깃발이 자꾸 무브포인트에 꼽히는 버그가 생겨서
            }
            yield return null;
        }


    }
    

// Use this for initialization
void Start()
    {
        StartCoroutine(makeFlag());
        test = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
