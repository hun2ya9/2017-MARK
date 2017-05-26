using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{

    public Transform Text; // 캔버스
    Text g;
    public GameObject hole;
    public GameObject house;
    public Transform item;
    Vector3[] hp; // 구멍 배열
    Vector2[] Lp; // 등대 배열
    Vector2[] ip; // 아이템 배열
    Vector2 housePosition;
    bool checkDLine = false; // 대각선 만들어지면 true값됨
    GameObject[] holes; // 구멍의 실제 오브젝트가 담기는 배열(hp[]는 그냥 위치가 담겨있는거임) =>holes는 실제 오브젝트의 위치 옮길때 쓰임 
    public Transform playerPoint; // 플레이어 위치

    //Transform ht1, ht2; // 위치를 조정한 구멍

    //추가 => 아이템에 사용됨
    GameObject[] newOneBlockPoint = new GameObject[9];
    public GameObject OneBlockPoint;
    public Transform ViewOneBlockPoint;// 그냥 위치를 저장하는 변수임 딱히 의미없음

    void HowManyHole() // 등대에 표시될 구멍의 개수
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++) // 등대의 개수만큼 반복
        {
            if (Lp[i] == Vector2.zero) { 
                continue;
            }

            int countH = 0; // 등대와 같은 x축 y축의 구멍개수 파악
            for (int j = 0; j < Mathf.RoundToInt(gridWorldSize.x) + 1; j++) // 구멍 개수만큼 반복
            {
                if (hp[Mathf.RoundToInt(gridWorldSize.x)] == Vector3.zero) // 마지막 배열위치에 구멍이 없을때는 = 대각선 안만들어졌을때
                {
                    if (j == Mathf.RoundToInt(gridWorldSize.x)) { // 마지막꺼 비교할 필요 없으니 넘긴다.

                    }
                    else if (Lp[i].x == hp[j].x || Lp[i].y == hp[j].y) // 같은 x축 혹은 y축 선상에 있을때
                    {
                        countH += 1;
                    }
                }
                else { // 마지막 구멍의 위치가 0,0가 아니다 라는 말은 대각선에 구멍이 하나도 없어서 새로 하나 만들었다. 라는소리임
                    if (Lp[i].x == hp[j].x || Lp[i].y == hp[j].y) // 같은 x축 혹은 y축 선상에 있을때
                    {
                        countH += 1;
                    }
                }

            }
            if (countH == 0) // 등대가 하나도 구멍과 안 겹칠수없음 == 띄어넘는다.
            {
                continue;
            }
            else // 제대로 등대가 기능한다.
            {
                g.text = countH + "";
                Vector2 L = new Vector3(Lp[i].x, Lp[i].y);

                Transform x = Instantiate(Text); // Text생성

                x.position = L;
            }

        }
    }

    private void Awake()
    {

        Trap();

    }
    void Start()
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        g = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Text>();
        HowManyHole();
        

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Trap()
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        //  Debug.Log(gridWorldSize);
        Vector2 left, right;
        int notCenter = 0;
        Vector2 holePosition;
        hp = new Vector3[Mathf.RoundToInt(gridWorldSize.x) + 1];
        //GameObject newhole;
        holes = new GameObject[Mathf.RoundToInt(gridWorldSize.x) + 1];
        for (int i = 0; i < gridWorldSize.x; i++)
        {
            int countt = 0;
            if (i == 0) // 첫줄에선 시작지점에 안 생기도록, 시작바로 옆에도 안생김
            {
                holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(2, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);
                GameObject newhole = Instantiate(hole);
                newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                hp[i] = new Vector2(holePosition.x, holePosition.y); // 배열에는 z축 0인걸로 저장 = 다른 오브젝트와 연동해야되니까
                holes[i] = newhole;
            }
            else if (i == gridWorldSize.x - 1) // 마지막줄에는 도착지점에 안 생기도록, 도착 바로옆에도 안생김
            {
                holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 2)), (gridWorldSize.y * 5) - 5 - 10 * i);

                // 마지막과 그 전의 구멍이 연속되지 않게끔
                if ((holePosition.y + 10 == hp[Mathf.RoundToInt(gridWorldSize.x - 2)].y) && (holePosition.x == hp[Mathf.RoundToInt(gridWorldSize.x - 2)].x)) // y축 10차이 나면서 x값 같은경우 = 연속 생성인 경우
                {

                    countt++;
                }
                if (countt > 0) // 아래위로 연속되면 다시 위치설정
                {
                    if (Mathf.RoundToInt(gridWorldSize.x) % 2 == 0) // 짝수일때
                    {
                        Debug.Log("구멍의위치" + holePosition.x);
                        if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) + 5)
                        { // 만약에 왼쪽 끄트머리면
                            holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x - 2)); // 오른쪽 아무대나로 보냄 근데 마지막이니까 도착지점 옆엔 안됨
                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄

                        }
                        else
                        { //끄트머리 아닐때 =>이게 굉장히 중요한데 오른쪽으로 보내버리면 도착 바로 옆지점으로 갈 수도 있기땜에
                          // holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                            holePosition.x -= 10; // 무조건 왼쪽으로 한칸 보내버림
                        }
                        GameObject newhole = Instantiate(hole);
                        newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);
                        holes[i] = newhole;
                    }
                    else
                    { // 홀수일때
                        if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) + 5)
                        { // 만약에 왼쪽 끄트머리면
                            holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 오른쪽 아무대나로 보냄

                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                        }
                        else
                        { //끄트머리 아닐때
                          // holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                            holePosition.x += 20 * Random.Range(0, 2) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                        }
                        GameObject newhole = Instantiate(hole);
                        newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);
                        holes[i] = newhole;

                    }
                }
                else
                { // 걍 연속안될때
                    GameObject newhole = Instantiate(hole);
                    newhole.transform.position = new Vector3(holePosition.x, holePosition.y, 1);
                    hp[i] = new Vector2(holePosition.x, holePosition.y);
                    holes[i] = newhole;

                }
            }
            else // 나머지 줄에 대한 생성인데 3개이상 연속되면 다른곳 배치하도록
            {
                holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);
                for (int j = 0; j < i; j++)
                { // i 가 0일땐 넘어가고 1일때 부터 그 이전의 위치와 비교
                    if ((holePosition.y + 10 == hp[j].y) && (holePosition.x == hp[j].x)) // y축 10차이 나면서 x값 같은경우 = 연속 생성인 경우
                    {

                        countt++;
                    }
                }
                if (countt > 0) // 아래위로 연속되면 다시 위치설정
                {
                    if (Mathf.RoundToInt(gridWorldSize.x) % 2 == 0) // 짝수일때
                    {
                        Debug.Log("구멍의위치" + holePosition.x);
                        if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) + 5)
                        { // 만약에 왼쪽 끄트머리면
                            holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 오른쪽 아무대나로 보냄
                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄

                        }
                        else
                        { //끄트머리 아닐때
                            holePosition.x += (20 * Random.Range(0, 2)) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                        }
                        GameObject newhole = Instantiate(hole);
                        newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                        hp[i] = new Vector2(holePosition.x, holePosition.y); holes[i] = newhole;

                    }
                    else
                    { // 홀수일때
                        if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) + 5)
                        { // 만약에 왼쪽 끄트머리면
                            holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 오른쪽 아무대나로 보냄
                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                        }
                        else
                        { //끄트머리 아닐때
                            holePosition.x += 20 * Random.Range(0, 2) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                        }
                        GameObject newhole = Instantiate(hole);
                        newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                        hp[i] = new Vector2(holePosition.x, holePosition.y); holes[i] = newhole;


                    }
                }
                else
                { // 걍 연속안될때
                    GameObject newhole = Instantiate(hole);
                    newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                    hp[i] = new Vector2(holePosition.x, holePosition.y); holes[i] = newhole;


                }
            }
        }

        // 대각선 방향에 아무것도 없으면 너무 쉬우니깐 최소 1개라도 만드려고
        for (int i = 0; i < gridWorldSize.x; i++) // 대각선 다른 위치에 대해서 모든 구멍과 비교
        {
            for (int j = 0; j < gridWorldSize.x; j++) // 대각선 한 위치에 대해서 모든 구멍과 비교
            {
                if (Mathf.RoundToInt(gridWorldSize.x) % 2 == 0) // 짝수일때 오른 대각선 확인
                {
                    right = new Vector2(5 * i, -5 * i); // (0,0) (5,-5),(10,-10),(15,-15), ... , (도착지점)
                    left = new Vector2(-5 * i, 5 * i); // 짝수 왼쪽 대각선 (0,0) (5,-5) (10,-10), (15, -15), (20, -20), (25, -25) // 홀수맵일땐 10씩

                }
                else
                {// 이건 홀수 일때 경우
                    right = new Vector2(10 * i, -10 * i); //홀수 오른쪽
                    left = new Vector2(-10 * i, 10 * i); // 홀수 왼쪽

                }

                if (hp[j].x == right.x && hp[j].y == right.y) // 오른쪽 아래 대각선에 위치하는 구멍 수 파악
                {
                    notCenter++;
                }
                if (hp[j].x == left.x && hp[j].y == left.y) // 왼쪽 아래 대각선에 위치하는 구멍 수 파악
                {
                    notCenter++;

                }
            }
        }
        if (notCenter == 0 && gridWorldSize.x > 6) // 대각선에 아무것도 없으면 생성 => 6x6까지는 안생기는걸로 함
        {
            int y = Random.Range(-Mathf.RoundToInt(gridWorldSize.x - 5), Mathf.RoundToInt(gridWorldSize.x - 4));
            int yy = Mathf.RoundToInt(y / 2);

            if (gridWorldSize.x % 2 == 0) // 짝수맵
            {

                if (y >= 0)
                {
                    holePosition = new Vector2(10 * yy - 5, 10 * (-yy) + 5); // 해봐야 6이면 -1 0 1 의경우 // 8이면 -3 -2 -1 0 1 2 3 => 어떻게든 -1 0 1 나옴
                }
                else
                {
                    holePosition = new Vector2(10 * yy + 5, 10 * (-yy) - 5);

                }
                GameObject newhole = Instantiate(hole);
                newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                hp[Mathf.RoundToInt(gridWorldSize.x)] = new Vector3(holePosition.x, holePosition.y); holes[Mathf.RoundToInt(gridWorldSize.x)] = newhole;
                checkDLine = true;

            }
            else
            { // 홀수맵 0,0에서 오류가 발생하므로 -10,10 이나 10,-10 에서만 만드는걸로

                if (y >= 0)
                {
                    holePosition = new Vector2(10, -10);
                }
                else
                {
                    holePosition = new Vector2(-10, 10);

                }
                GameObject newhole = Instantiate(hole);
                newhole.transform.position = new Vector3(holePosition.x, holePosition.y,1);
                hp[Mathf.RoundToInt(gridWorldSize.x)] = new Vector3(holePosition.x, holePosition.y); holes[Mathf.RoundToInt(gridWorldSize.x)] = newhole;
                checkDLine = true;

            }
        }


        // 감시탑 랜덤 생성

        GameObject newHouse;
        int count = 0;
        Lp = new Vector2[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)]; // 여기에 감시탑의 실제 위치를 저장
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++)
        {
            int except = 0;
            int m = Random.Range(0, Mathf.RoundToInt(gridWorldSize.x));
            if (m == 0)
            {
                housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }
            else if (m == gridWorldSize.x - 1)
            {
                housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * m);
            }
            else if (m == Mathf.FloorToInt(gridWorldSize.x / 2)) // 7은 4, 9는 5, 11은 6, 13는 7 == 딱 절반의 배열위치
            {// 이걸 추가하는 이유는 등대가 0,0에 생성되지 않도록 하기위함 = null값이 0,0이라서 중복오류발생
                if (Mathf.RoundToInt(gridWorldSize.x % 2) == 0) // 짝수는 상관없으니 생성
                {
                    housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);
                }
                else
                { //홀수는 생기면 안됨 Mathf.FloorToInt(gridWorldSize.x/2)+1 위치만 피해가면됨
                    int random1 = Random.Range(0, Mathf.FloorToInt(gridWorldSize.x / 2));
                    int random2 = Random.Range(Mathf.FloorToInt(gridWorldSize.x / 2) + 2, Mathf.RoundToInt(gridWorldSize.x));
                    int k = Random.Range(0, 2) == 0 ? random1 : random2; // 0이나 1 랜덤으로 뽑아서 0이면 random1값 1이면 random2값 넣음
                    housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * k, (gridWorldSize.y * 5) - 5 - 10 * m);
                }
            }
            else
            {
                housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }


            // 구멍과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x + 1; j++) // 1을 더 더해주는 이유는 대각선 없을시 하나 생성하기 때문
            {
                if (housePosition.x == hp[j].x && housePosition.y == hp[j].y) // 현재 임의의 감시탑 위치와 구멍이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                }
            }

            for (int k = 0; k < i; k++) // i가 0일때는 비교안하고 바로 넘어가고 아니면 그 전에 것과 비교함
            {
                if (Lp[i] != null) // 생성된 것이 없을땐 null , 있으면 실행
                {
                    if ((housePosition.x != Lp[k].x) && (housePosition.y != Lp[k].y)) // x축 y축 겹치지 않으면 
                    {// 생성 => 이렇게 하면 생성안되고 넘어간 경우는 비교하지 않고 생성된것과 비교
                    }
                    else
                    {
                        except++; // 겹치면 안됨
                    }
                    // 대각선 좌상 우상 좌하 우하 연속되게 겹치지 못하게끔
                    if ((housePosition.x - 10 == Lp[k].x) && (housePosition.y - 10 == Lp[k].y))
                    {
                        except++;
                    }
                    else if ((housePosition.x - 10 == Lp[k].x) && (housePosition.y + 10 == Lp[k].y))
                    {
                        except++;
                    }
                    else if ((housePosition.x + 10 == Lp[k].x) && (housePosition.y + 10 == Lp[k].y))
                    {
                        except++;
                    }
                    else if ((housePosition.x + 10 == Lp[k].x) && (housePosition.y - 10 == Lp[k].y))
                    {
                        except++;
                    }
                }
            }

            if (except == 0) // 등대는 따질것이 많기때문에 배열에 순차적으로(0,1,2,3..) 들어가는게 아니라 배열 중간중간에 들어가게됨
            {
                newHouse = Instantiate(house);
                newHouse.transform.position = new Vector2(housePosition.x, housePosition.y);
                count += 1;

                Lp[i] = new Vector2(housePosition.x, housePosition.y); // 생성된 위치는 Rhu 배열에 넣음

            }
            if (count == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                break;
            }
        }
            

            // 등대 주변 8방위 구멍 위치 z = -1으로 조정해야 보임
            
            for (int ii = 0; ii < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); ii++) // 등대의 개수만큼 반복
            {
                if (Lp[ii] == Vector2.zero) // 등대 0,0 일때 패스
                {
                    continue;
                }
                else
                {
                    for (int k = 0; k < Mathf.RoundToInt(gridWorldSize.x); k++) // 구멍의 개수만큼 반복
                    {
                        Transform ht2 = holes[k].GetComponent<Transform>();

                        for (int y = -1; y < 2; y++)
                        {
                            for (int z = -1; z < 2; z++)
                            {
                                if (Lp[ii].x - 10 * y == ht2.position.x && Lp[ii].y - 10 * z == ht2.position.y)
                                {
                                    ht2.position = new Vector3(holes[k].transform.position.x, holes[k].transform.position.y, 0);
                                }
                            }
                        } // 각 등대위치에서 8방위 채크해서 구멍이 있으면 z축 0으로 만들어서 보이게끔해줌

                    }

                    if (checkDLine == true) // 마지막 배열위치에 구멍이 없을때는 = 대각선 안만들어졌을때
                    {
                        Transform ht1 = holes[Mathf.RoundToInt(gridWorldSize.x)].GetComponent<Transform>();

                        for (int y = -1; y < 2; y++)
                        {
                            for (int z = -1; z < 2; z++)
                            {
                                if (Lp[ii].x - 10 * y == ht1.position.x && Lp[ii].y - 10 * z == ht1.position.y)
                                {
                                    ht1.position = new Vector3(holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.x, holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.y, 0);
                                }
                            }
                        } // 각 등대위치에서 8방위 채크해서 구멍이 있으면 z축 0으로 만들어서 보이게끔해줌

                    }
                }
            }
        

        Vector2 itemPosition;
        int count2 = 0;
        // hu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)];
        ip = new Vector2[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)]; // 여기에 아이템의 실제 위치를 저장
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++)
        {
            int except = 0;
            int m = Random.Range(0, Mathf.RoundToInt(gridWorldSize.x));
            if (m == 0)
            {
                itemPosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }
            else if (m == gridWorldSize.x - 1)
            {
                itemPosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * m);
            }
            else
            {
                itemPosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }

            // 구멍과 감시탑과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x + 1; j++)
            {
                if (itemPosition.x == hp[j].x && itemPosition.y == hp[j].y) // 현재 임의의 아이템 위치와 구멍이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                }
            }
            for (int j = 0; j < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); j++)
            {

                if (itemPosition.x == Lp[j].x && itemPosition.y == Lp[j].y) // 현재 임의의 아이템 위치와 감시탑이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                }
            }


            for (int k = 0; k < i; k++) // i가 0일때는 비교안하고 바로 넘어가고 아니면 그 전에 것과 비교함
            {
                if (ip[i] != null) // 생성된 것이 없을땐 null 있으면 실행
                {
                    if ((itemPosition.x != ip[k].x) && (itemPosition.y != ip[k].y)) // x축 y축 겹치지 않으면 
                    {// 생성 => 이렇게 하면 생성안되고 넘어간 경우는 비교하지 않고 생성된것과 비교

                    }
                    else
                    {
                        except++; // 겹치면 안됨
                    }


                }
            }

            if (except == 0)
            {
                Transform newItem = Instantiate(item);
                newItem.position = new Vector2(itemPosition.x, itemPosition.y);
                count2 += 1;
                ip[i] = new Vector2(itemPosition.x, itemPosition.y);// 생성된 위치는 ip 배열에 넣음
            }


            if (count2 == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                break;
            }
        }

        

    }

    
    
    public void Z2() // 코드가 조잡해져서 이러기 싫었지만.... Z축구분을 위해서 이쪽으로 넣을 수 밖에 없었다.
    {
        // 1. 맵 범위 안에서 2. 현재 플레이어 위치 중심 3. 8방위 조건만족하면 타일 생성하도록, 등대에는 생성되면 안됨!
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        // 무브포인트의 위치 = 타일 생성하는 곳임
        int n = 10;
        Vector2 over = new Vector2(gridWorldSize.x * 5, gridWorldSize.y * 5); // 맵 사이즈 저장


        for (int i = 1; i < 4; i++) // 123   369 - 123  = 210, 543, 876 하면 배열 0부터 8번까지 저장
            { // 8방위
                for (int j = 1; j < 4; j++) // 123
                {
                    int countL = 0;
                
                Vector2 MPpoisition = new Vector2(playerPoint.position.x + n * (i - 2), playerPoint.position.y + n * (j - 2)); // -1, 0, 1
                                     // 타일 깔 위치는 for문 돌릴때마다 위치 바껴야해서

                if (i == 2 && j == 2) // 중심에는 만들 필요 없음
                    {
                    continue;
                    }
                    for (int z = 0; z < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); z++)
                    { // 무브 포인트와 등대위치가 일치히면 예외카운트 + 이말은 생성 안한다는 말임
                    if (Lp[z] == Vector2.zero) { // 0,0에는 생길 수가 없음 0,0은 null값임
                        continue;
                    } 
                    if (Lp[z] == MPpoisition)
                        {
                            countL++;
                        }
                    }
                if (checkDLine == false) // 마지막 배열위치에 구멍이 없을때는 = 대각선 안만들어졌을때
                {
                    for (int z = 0; z < Mathf.RoundToInt(gridWorldSize.x); z++) // 구멍 수만큼 반복하면 됨
                    {
                        Debug.Log(holes[z].transform.position.x + "이게 문제냐 null?");
                        if (holes[z].transform.position.x == MPpoisition.x && holes[z].transform.position.y == MPpoisition.y) // 무브 포인트와 구멍위치 일치
                        {
                            if (holes[z].transform.position.z == 0) // x,y같은데 z가 0이면 만들면안됨
                            {
                                Debug.Log(holes[z].transform.position + "이거 확인");
                                countL++;
                            }
                        }
                    }
                }
                else {
                    for (int z = 0; z < Mathf.RoundToInt(gridWorldSize.x)+1; z++) // 구멍 수만큼 반복하면 됨
                    {
                        Debug.Log(holes[z].transform.position + "이게 문제냐 null(아님)?");
                        if (holes[z].transform.position.x == MPpoisition.x && holes[z].transform.position.y == MPpoisition.y) // 무브 포인트와 구멍위치 일치
                        {
                            if (holes[z].transform.position.z == 0) // x,y같은데 z가 0이면 만들면안됨
                            {
                                Debug.Log(holes[z].transform.position + "이거 확인");
                                countL++;
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
                                newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

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
                                newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);

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
                                newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
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
                                newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                            // 홀수맵에서는 좌표가 0일 수가 있음;; x,y가 하나이상 0인 경우임
                        }
                        else if (MPpoisition.x == 0)// x가 0일 경우
                    { 

                            if (MPpoisition.y > 0) // 위쪽
                            {
                                if (MPpoisition.y > over.y)
                                {
                                    continue; // 맵 벗어나면 만들지 않는다.
                                }
                                else // 여기까지 왔으면 맵 안이라는 소리임
                                {
                                    newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                    newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
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
                                    newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                    newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                                }
                            }
                            else
                        { // 딱 중심일때  = 0,0 이라는 말임
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }

                        }
                        else if (MPpoisition.y == 0) // y가 0일 경우
                    { 
                            if (MPpoisition.x > 0) // 오른쪽
                            {
                                if (MPpoisition.x > over.x)
                                {
                                    continue; // 맵 벗어나면 만들지 않는다.
                                }
                                else // 여기까지 왔으면 맵 안이라는 소리임
                                {
                                    newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                    newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
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
                                    newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                    newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                                }
                            }
                            else
                            { // 딱 중심일때 = 0,0 이라는 말임
                                newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                                newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                            }
                        }
                    else  //= 0,0 이라는 말임
                        {
                            newOneBlockPoint[i * 3 - j] = Instantiate(OneBlockPoint); // 생성
                            newOneBlockPoint[i * 3 - j].transform.position = new Vector3(MPpoisition.x, MPpoisition.y);
                        }
                    }
                }
            }
        
    }
    public void DestroyNewOneBlockPoint()
    {
        for (int i = 0; i < 9; i++)
        {
            DestroyObject(newOneBlockPoint[i]); // 기존 위치 제거
        }
    }

    public void OBD() // 임의의 위치와 구멍 위치가 동일하다면 구멍이 눈에 보이게끔
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
       
        DestroyNewOneBlockPoint(); //굳이 코루틴 안써도 될것같아서 => 이거는 깔았던 타일 없애는작업임
        Debug.Log("4단계");
       // Item h = new Item();
        //Transform ViewOneBlockPoint = h.ViewOneBlockPoint; 
        // 이런 방법으론 값을 못가져옴 => 그래서 그냥 public으로 유니티상에서 오브젝트 끌어다 넣었음

        for (int k = 0; k < Mathf.RoundToInt(gridWorldSize.x); k++) // 구멍의 개수만큼 반복
        {
            Transform ht2 = holes[k].GetComponent<Transform>(); // holes 의 위치정보를 따로 ht2변수에 저장
            //아직도 ht2.transform.position 와 ht2.position 의 차이는 모르겠음 똑같은거라 추측중임

            if (ViewOneBlockPoint.position == ht2.position) // 임의의 위치와 구멍의 위치가 일치한다
            {
                ht2.position = new Vector3(holes[k].transform.position.x, holes[k].transform.position.y, 0);

            }

        }
        if (checkDLine == true) // 대각선 만들어졌을때
        {
            Transform ht1 = holes[Mathf.RoundToInt(gridWorldSize.x)].GetComponent<Transform>();
            if (ViewOneBlockPoint.position == ht1.position)
            {
                ht1.position = new Vector3(holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.x, holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.y, 0);
            }
        }
       // yield return null;
    }


    public void EBD() // 플레이어 중심으로 8방위 위치에 구멍이 있다면 
    {

        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        Vector2 ViewEightBlockPoint;

        for (int i = 1; i < 4; i++) 
        { // 8방위
            for (int j = 1; j < 4; j++) // 123
            {
                // ViewEightBlockPoint = 
               ViewEightBlockPoint = new Vector2(playerPoint.position.x + 10 * (i - 2), playerPoint.position.y + 10 * (j - 2));
         

        for (int k = 0; k < Mathf.RoundToInt(gridWorldSize.x); k++) // 구멍의 개수만큼 반복
        {
            Transform ht2 = holes[k].GetComponent<Transform>(); // holes 의 위치정보를 따로 ht2변수에 저장
            if (ViewEightBlockPoint.x == ht2.position.x && ViewEightBlockPoint.y == ht2.position.y) // 플레이어 중심 8방위위치 하나와 구멍의 위치가 일치한다
            {
                ht2.position = new Vector3(holes[k].transform.position.x, holes[k].transform.position.y,0); // 일치하면 보이게끔

            }

        }
        if (checkDLine == true) // 대각선 만들어졌을때
        {
            Transform ht1 = holes[Mathf.RoundToInt(gridWorldSize.x)].GetComponent<Transform>();
            if (ViewEightBlockPoint.x == ht1.position.x && ViewEightBlockPoint.y == ht1.position.y)
            {
                ht1.position = new Vector3(holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.x, holes[Mathf.RoundToInt(gridWorldSize.x)].transform.position.y, 0);
            }
        }
    }
    }
    }
}