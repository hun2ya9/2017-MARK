using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hole: MonoBehaviour
{
    public Transform hole;
    public GameObject house;
    //public Transform house;
    public Transform item;

    Vector3[] hp;
    public Vector3[] Lp;
    Vector3[] ip; // 아이템 배열
    Vector3 housePosition;
    // Use this for initialization
    
    void Start()
    {
        Trap();

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
        int e = 5;
        int od = 10;
        Vector3 left, right;
        int notCenter = 0;
        int countt = 0;
        Vector3 holePosition;
        hp = new Vector3[Mathf.RoundToInt(gridWorldSize.x)+1];

        for (int i = 0; i < gridWorldSize.x; i++)
        {
            if (i == 0)
            {
                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);
                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                hp[i] = new Vector3(holePosition.x, holePosition.y);

            }
            else if (i == gridWorldSize.x - 1)
            {
                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * i);
                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                hp[i] = new Vector3(holePosition.x, holePosition.y);

            }
            else // 3개이상 연속되면 다른곳 배치하도록
            //근데 여기서 문제는 3번째줄에 길을 막아버리는 예외가 생길수도있음 x가 -5일때는 더욱더 빡시게
            {
                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);

                for (int j = 0; j < i; j++)

                   if ((holePosition.y + od == hp[i].y) && (holePosition.x == hp[i].x)) // y축 10차이 나면서 x값 같은경우
                    {
                        countt++;
                        if (holePosition.x == -od) // 연속되는데 -5의 위치였다
                        {
                            countt += 2; // 바로 카운트 2를 더 올리며 3이된다.
                        }
                    } 
                if (countt > 2)
                {
                    holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                }

                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                hp[i] = new Vector3(holePosition.x, holePosition.y);

            }
            }
        // 대각선 방향에 아무것도 없으면 너무 쉬우니깐 최소 1개라도 만드려고
        for (int i = 0; i < gridWorldSize.x; i++) // 대각선 다른 위치에 대해서 모든 구멍과 비교
        {
            for (int j = 0; j < gridWorldSize.x; j++) // 대각선 한 위치에 대해서 모든 구멍과 비교
            {
                if (gridWorldSize.x % 2 == 0)
                {
                    right = new Vector3(e * i, -e * i); // 왼쪽 대각선 0,0 5,-5 10,-10, 15, -15, 20, -20, 25, -25 // 홀수맵일땐 10씩
                }
                else {
                    right = new Vector3(od * i, -od* i);
                }


                    if (hp[j].x == right.x && hp[j].y == right.y)
                {
                    notCenter++;
                }


                if (gridWorldSize.x % 2 == 0)
                {
                    left = new Vector3(-e * i, e * i); // 오른쪽 대각선
                } else
                {
                    left = new Vector3(od * i, -od * i);
                }


                    if (hp[j].x == left.x && hp[j].y == left.y)
                {
                    notCenter++;
                }
            }
            if (notCenter == 0) // 한 구멍이 대각선의 한 위치와 일치하면 하나 만들고
            {
                int y = Random.Range(-Mathf.RoundToInt(gridWorldSize.x - 5), Mathf.RoundToInt(gridWorldSize.x - 4));
                int yy = Mathf.RoundToInt(y / 2);

                if (gridWorldSize.x % 2 == 0) // 짝수맵
                {

                    if (y >= 0)
                    {
                        holePosition = new Vector3(10 * yy - 5, 10 * (-yy) + 5); // 해봐야 6이면 -1 0 1 의경우 // 8이면 -3 -2 -1 0 1 2 3 => 어떻게든 -1 0 1 나옴
                    }
                    else
                    {
                        holePosition = new Vector3(10 * yy + 5, 10 * (-yy) - 5);

                    }
                    Transform newhole = Instantiate(hole);
                    newhole.position = new Vector3(holePosition.x, holePosition.y);
                    hp[Mathf.RoundToInt(gridWorldSize.x) + i] = new Vector3(holePosition.x, holePosition.y);
                    break;
                }
                else { // 홀수맵

                    if (y >= 0)
                    {
                        holePosition = new Vector3(10 * yy - 10, 10 * (-yy) + 10); // 해봐야 6이면 -1 0 1 의경우 // 8이면 -3 -2 -1 0 1 2 3 => 어떻게든 -1 0 1 나옴
                    }
                    else
                    {
                        holePosition = new Vector3(10 * yy + 10, 10 * (-yy) - 10);

                    }
                    Transform newhole = Instantiate(hole);
                    newhole.position = new Vector3(holePosition.x, holePosition.y);
                    hp[Mathf.RoundToInt(gridWorldSize.x) + i] = new Vector3(holePosition.x, holePosition.y);
                    break;
                }



            }
        }

        // 감시탑 랜덤 생성
        GameObject newHouse;
        int count = 0;
        // hu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)];
        Lp = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)]; // 여기에 감시탑의 실제 위치를 저장
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++)
        {
            int except = 0;
            
            int m = Random.Range(0, Mathf.RoundToInt(gridWorldSize.x));
            //Vector3 housePosition;
            if (m == 0)
            {
                housePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }
            else if (m == gridWorldSize.x - 1)
            {
                housePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * m);
            }
            else
            {
                housePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }

            // 시작,끝 위치에 겹치는가?
            if (housePosition.x == -(gridWorldSize.x * 5) + 5 && housePosition.y == (gridWorldSize.y * 5) - 5)
            {
                except++;
            }
            if (housePosition.x == (gridWorldSize.x * 5) - 5 && housePosition.y == -(gridWorldSize.y * 5) + 5)
            {
                except++;
            }
            // 구멍과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x; j++)
            {
                if (housePosition.x == hp[j].x && housePosition.y == hp[j].y) // 현재 임의의 감시탑 위치와 구멍이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                }
            }

            // hu[i] = housePosition; // 일단 배열에 넣는다.

            for (int k = 0; k < i; k++) // i가 0일때는 비교안하고 바로 넘어가고 아니면 그 전에 것과 비교함
            {
                if (Lp[i] != null) // 생성된 것이 없을땐 null 있으면 실행
                {
                    if ((housePosition.x != Lp[k].x) && (housePosition.y != Lp[k].y)) // x축 y축 겹치지 않으면 
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
                newHouse = Instantiate(house);
                newHouse.transform.position = new Vector3(housePosition.x, housePosition.y);
                count += 1;

                Lp[i] = new Vector3(housePosition.x, housePosition.y); // 생성된 위치는 Rhu 배열에 넣음
                
            }
            if (count == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                break;
            }
            
        }


        Vector3 itemPosition;
        int count2 = 0;
        // hu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)];
        ip = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)]; // 여기에 아이템의 실제 위치를 저장
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++)
        {
            int except = 0;
            int m = Random.Range(0, Mathf.RoundToInt(gridWorldSize.x));
            if (m == 0)
            {
                itemPosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

            }
            else if (m == gridWorldSize.x - 1)
            {
                itemPosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * m);
            }
            else
            {
                itemPosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * m);

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
                newItem.position = new Vector3(itemPosition.x, itemPosition.y);
                count2 += 1;
                ip[i] = new Vector3(itemPosition.x, itemPosition.y);// 생성된 위치는 ip 배열에 넣음
            }


            if (count2 == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                break;
            }
        }

    }
    

}



