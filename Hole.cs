using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Hole: MonoBehaviour
{
    public Transform hole;
    public Transform house;
    public Transform item;
    Vector3[] h;
    Vector3[] Rhu;
    Vector3[] ip; // 아이템 배열

    // Use this for initialization
    void Start()
    {
        Trap();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Trap()
    {
        ObjectManager o = new ObjectManager();
           Vector2 gridWorldSize = o.getSize();

        Debug.Log(gridWorldSize);
        int n = 5;
        Vector3 left, right;
        int notCenter = 0;
        int countt = 0;
        Vector3 holePosition;
        h = new Vector3[Mathf.RoundToInt(gridWorldSize.x)];

        for (int i = 0; i < gridWorldSize.x; i++)
        {
            if (i == 0)
            {
                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(1, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);
                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                h[i] = new Vector3(holePosition.x, holePosition.y);

            }
            else if (i == gridWorldSize.x - 1)
            {
                // 여기서 같은 열에 3개이상못있도록 만드는게 좋을것같은데

                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x - 1)), (gridWorldSize.y * 5) - 5 - 10 * i);
                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                h[i] = new Vector3(holePosition.x, holePosition.y);

            }
            else
            {
                holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i);

                for (int j = 0; j < i; j++)
                    if ((holePosition.y + n == h[i].y) && (holePosition.x == h[i].x)) // y축 5차이 나면서 x값 같은경우
                    {
                        countt++;
                    }
                if (countt > 2)
                {
                    holePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * i); //위치 다시 할당
                }


                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                h[i] = new Vector3(holePosition.x, holePosition.y);


            }

            right = new Vector3(n * i, -n * i);
            if (h[i].x == right.x && h[i].y == right.y)
            {
                notCenter++;
            }

            left = new Vector3(-n * i, n * i);
            if (h[i].x == left.x && h[i].y == left.y)
            {
                notCenter++;
            }

            if (notCenter == 0)
            {
                int y = Random.Range(-Mathf.RoundToInt(gridWorldSize.x - 4), Mathf.RoundToInt(gridWorldSize.x - 4));
                int yy = Mathf.RoundToInt(y / 2);
                if (y > 0)
                {
                    holePosition = new Vector3(10 * yy - 5, 10 * (-yy) + 5);
                }
                else
                {
                    holePosition = new Vector3(10 * yy + 5, 10 * (-yy) - 5);

                }
                Transform newhole = Instantiate(hole);
                newhole.position = new Vector3(holePosition.x, holePosition.y);
                h[i] = new Vector3(holePosition.x, holePosition.y);
            }
        }

        int count = 0;
        // hu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)];
        Rhu = new Vector3[Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x)]; // 여기에 감시탑의 실제 위치를 저장
        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x * gridWorldSize.x); i++)
        {
            int except = 0;

            //랜덤 위치 생성

            //  Vector3 a = new Vector3(-(gridWorldSize.x * 5) + 5, (gridWorldSize.y * 5) - 5);
            //Vector3 b = new Vector3((gridWorldSize.x * 5) - 5 , -(gridWorldSize.y * 5) + 5);

            //Vector3 housePosition = new Vector3(-(gridWorldSize.x * 5) + 5 + 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.x)), (gridWorldSize.y * 5) - 5 - 10 * Random.Range(0, Mathf.RoundToInt(gridWorldSize.y)));

            int m = Random.Range(0, Mathf.RoundToInt(gridWorldSize.x));
            Vector3 housePosition;
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
                except++; // 겹치면 안됨
                Debug.Log("겹쳤다1");
            }
            if (housePosition.x == (gridWorldSize.x * 5) - 5 && housePosition.y == -(gridWorldSize.y * 5) + 5)
            {
                except++; // 겹치면 안됨
                Debug.Log("겹쳤다2");
            }
            // 구멍과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x; j++)
            {
                if (housePosition.x == h[j].x && housePosition.y == h[j].y) // 현재 임의의 감시탑 위치와 구멍이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                    Debug.Log("겹쳤다3");
                }
            }

            // hu[i] = housePosition; // 일단 배열에 넣는다.

            for (int k = 0; k < i; k++) // i가 0일때는 비교안하고 바로 넘어가고 아니면 그 전에 것과 비교함
            {
                if (Rhu[i] != null) // 생성된 것이 없을땐 null 있으면 실행
                {
                    if ((housePosition.x != Rhu[k].x) && (housePosition.y != Rhu[k].y)) // x축 y축 겹치지 않으면 
                    {// 생성 => 이렇게 하면 생성안되고 넘어간 경우는 비교하지 않고 생성된것과 비교

                    }
                    else
                    {
                        except++; // 겹치면 안됨
                        Debug.Log("겹쳤다4");
                    }


                }
            }


            //bool b = false;
            /* for (int j = 0; j < gridWorldSize.x; j++)
             { //if (b == false){ // b값이 false면 계속
                 if (count == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
                 {
                     break;
                 }
                 if (hu[i] == h[j]) // 현재 임의의 감시탑 위치와 구멍이 겹치는지 확인
                 {
                     except++; // 전부다 확인 하고 넘어가야됨
                 }
             }*/
            if (except == 0)
            {
                Transform newHouse = Instantiate(house);
                newHouse.position = new Vector3(housePosition.x, housePosition.y);
                count += 1;
                Debug.Log("생성 " + i);
                Debug.Log(count);

                Rhu[i] = housePosition; // 생성된 위치는 Rhu 배열에 넣음



            }
            if (count == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                Debug.Log("끝");
                break;
            }

            Debug.Log("반복");
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

            // 감시탑과 겹치는가 ?
            for (int j = 0; j < gridWorldSize.x; j++)
            {
                if (itemPosition.x == Rhu[j].x && itemPosition.y == Rhu[j].y) // 현재 임의의 아이템 위치와 감시탑이 겹치는지 확인
                {
                    except++; // 전부다 확인 하고 넘어가야됨
                    Debug.Log("아이템 감시탑 겹침");
                }
            }


            for (int k = 0; k < i; k++) // i가 0일때는 비교안하고 바로 넘어가고 아니면 그 전에 것과 비교함
            {
                if (ip[i] != null) // 생성된 것이 없을땐 null 있으면 실행
                {
                    if ((itemPosition.x != Rhu[k].x) && (itemPosition.y != Rhu[k].y)) // x축 y축 겹치지 않으면 
                    {// 생성 => 이렇게 하면 생성안되고 넘어간 경우는 비교하지 않고 생성된것과 비교

                    }
                    else
                    {
                        except++; // 겹치면 안됨
                        Debug.Log("아이템 겹쳤다");
                    }


                }
            }

            if (except == 0)
            {
                Transform newItem = Instantiate(item);
                newItem.position = new Vector3(itemPosition.x, itemPosition.y);
                count2 += 1;
                Debug.Log(count);

                ip[i] = itemPosition; // 생성된 위치는 ip 배열에 넣음

            }


            if (count2 == Mathf.RoundToInt(gridWorldSize.x / 2)) // 맵 크기 절반의 버림 갯수만큼 생성
            {
                Debug.Log("끝");
                break;
            }

        }

    }
}



