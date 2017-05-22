using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole: MonoBehaviour
{

    public Transform Text; // 캔버스
    Text g;


    public GameObject hole;
    public GameObject house;
    //public Transform house;
    public Transform item;
    Vector2[] hp; // 구멍 배열
    Vector2[] Lp; // 등대 배열
    Vector2[] ip; // 아이템 배열
    Vector2 housePosition;
    
    /* 등대 8방위는 구멍 보이게끔 하려는데
     처음 아이디어 : 구멍 8방위 layer 변경해서 그 layer는 culling mask에 보이게 => 이게 잘안됨. Awake에 넣으면 전부다 변경되고 따로 하면 프리팹 원본만 변경되고*/


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
                if (hp[Mathf.RoundToInt(gridWorldSize.x)] == Vector2.zero) // 마지막 구멍이 없을때는
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
        hp = new Vector2[Mathf.RoundToInt(gridWorldSize.x)+1];

        GameObject newhole;
        for (int i = 0; i < gridWorldSize.x; i++)
        {
            int countt = 0;
            if (i == 0) // 첫줄에선 시작지점에 안 생기도록, 시작바로 옆에도 안생김
            {
                holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(2, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);
                newhole = Instantiate(hole);
                newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                hp[i] = new Vector2(holePosition.x, holePosition.y);

            }
            else if (i == gridWorldSize.x - 1) // 마지막줄에는 도착지점에 안 생기도록, 도착 바로옆에도 안생김
            {
                holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 2)), (gridWorldSize.y * 5) - 5 - 10 * i);

                // 마지막과 그 전의 구멍이 연속되지 않게끔
                if ((holePosition.y + 10 == hp[Mathf.RoundToInt(gridWorldSize.x-2)].y) && (holePosition.x == hp[Mathf.RoundToInt(gridWorldSize.x-2)].x)) // y축 10차이 나면서 x값 같은경우 = 연속 생성인 경우
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
                            holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x-2)); // 오른쪽 아무대나로 보냄 근데 마지막이니까 도착지점 옆엔 안됨
                            Debug.Log("왼끄");
                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                            Debug.Log("오끄");

                        }
                        else
                        { //끄트머리 아닐때 =>이게 굉장히 중요한데 오른쪽으로 보내버리면 도착 바로 옆지점으로 갈 수도 있기땜에
                          // holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                            holePosition.x -= 10; // 무조건 왼쪽으로 한칸 보내버림
                            Debug.Log("논끄");
                        }
                        newhole = Instantiate(hole); // newhole하니 안되서 2로 일단바꿈
                        newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);
                    }
                    else
                    { // 홀수일때
                            if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) +5)
                            { // 만약에 왼쪽 끄트머리면
                                holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 오른쪽 아무대나로 보냄
                                Debug.Log("왼끄");

                            }
                            else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) -5)
                            { // 만약에 오른쪽 끄트머리면
                                holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                                Debug.Log("오끄");
                            }
                            else
                            { //끄트머리 아닐때
                              // holePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                                holePosition.x += 20 * Random.Range(0, 2) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                                Debug.Log("논끄");
                            }
                        newhole = Instantiate(hole);
                        newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);

                    }
                }
                else
                { // 걍 연속안될때
                    newhole = Instantiate(hole);
                    newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                    hp[i] = new Vector2(holePosition.x, holePosition.y);

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
                            Debug.Log("왼끄");
                        }
                        else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) - 5)
                        { // 만약에 오른쪽 끄트머리면
                            holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                            Debug.Log("오끄");

                        }
                        else
                        { //끄트머리 아닐때
                            holePosition.x += (20 * Random.Range(0, 2)) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                            Debug.Log("논끄");
                        }
                        newhole = Instantiate(hole);
                        newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);
                    }
                    else
                    { // 홀수일때
                            if (holePosition.x == -Mathf.RoundToInt(gridWorldSize.x * 5) +5)
                            { // 만약에 왼쪽 끄트머리면
                                holePosition.x += 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 오른쪽 아무대나로 보냄
                                Debug.Log("왼끄");
                            }
                            else if (holePosition.x == Mathf.RoundToInt(gridWorldSize.x * 5) -5)
                            { // 만약에 오른쪽 끄트머리면
                                holePosition.x -= 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)); // 왼쪽 아무대나로 보냄
                                Debug.Log("오끄");
                            }
                            else
                            { //끄트머리 아닐때
                                holePosition.x += 20 * Random.Range(0, 2) - 10; // 걍 오른쪽이나 왼쪽으로 한칸보냄 0일때 왼쪽한칸 1일때 
                                Debug.Log("논끄");
                            }
                        newhole = Instantiate(hole);
                        newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                        hp[i] = new Vector2(holePosition.x, holePosition.y);

                    }
                }
                else { // 걍 연속안될때
                    newhole = Instantiate(hole);
                    newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                    hp[i] = new Vector2(holePosition.x, holePosition.y);

                }
            }
        }

        /* 예외 처리 = > 시작,도착지점을 구멍2개 등대1개 혹은 등대2개 구멍1개로 막는 경우가 생길 수 있음
         * 해결책 => 아예 */




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
            if (notCenter == 0 && gridWorldSize.x >6) // 대각선에 아무것도 없으면 생성 => 6x6까지는 안생기는걸로 함
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
                    newhole = Instantiate(hole);
                    newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                    hp[Mathf.RoundToInt(gridWorldSize.x)] = new Vector3(holePosition.x, holePosition.y);
                    
                }
                else { // 홀수맵 0,0에서 오류가 발생하므로 -10,10 이나 10,-10 에서만 만드는걸로

                    if (y >= 0)
                    {
                        holePosition = new Vector2(10, -10);
                    }
                    else
                    {
                        holePosition = new Vector2(-10,10);

                    }
                   newhole = Instantiate(hole);
                    newhole.transform.position = new Vector2(holePosition.x, holePosition.y);
                    hp[Mathf.RoundToInt(gridWorldSize.x)] = new Vector3(holePosition.x, holePosition.y);
                 
                }
            }


        // 감시탑 랜덤 생성


        GameObject newHouse;
        int count = 0;
        // hu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)];
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
                else
                {
                    housePosition = new Vector2(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

                }
            

            // 구멍과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x+1; j++) // 1을 더 더해주는 이유는 대각선 없을시 하나 생성하기 때문
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
            for (int j = 0; j < gridWorldSize.x+1; j++)
            {
                if (itemPosition.x ==Lp[j].x && itemPosition.y == Lp[j].y) // 현재 임의의 아이템 위치와 감시탑이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                }
                if (itemPosition.x == hp[j].x && itemPosition.y == hp[j].y) // 현재 임의의 아이템 위치와 감시탑이 겹치는지 확인
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
    

}



