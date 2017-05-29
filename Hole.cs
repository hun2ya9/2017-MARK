using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//기존의 howmanyholes는 player클래스로, viewlight는 lighthouse클래스로 이동
//start의 고질적인 순서 문제 질문...스크립트 실행 순서 설정으로 해결
public class Hole : MonoBehaviour
{
    public GameObject hole;
    GameObject newhole;
    public GameObject object_manager;
    ObjectManager_ script;
    public GameObject[] holes; // 구멍의 실제 오브젝트가 담기는 배열(hp[]는 그냥 위치가 담겨있는거임) =>holes는 실제 오브젝트의 위치 옮길때 쓰임 

    void Start()
    {
        Trap();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(script.exist_root==true)
           touchedtrap();
    }

    public void Trap()
    {
        script = object_manager.GetComponent<ObjectManager_>();
        int i = 0, x = 0, y = 0;
        int mapsize= Mathf.RoundToInt(script.gridWorldSize.x);
        int hole_cnt = mapsize; //맵 한 줄의 길이==설치할 구멍의 수 
        holes = new GameObject[mapsize];
        int h = 0;
        do
        {
            x = Random.Range(0, mapsize- 1);
            y = Random.Range(0, mapsize - 1);
            if (script.grid[x, y].is_trap == false)  //트랩 이미 설치되있냐
            {
                int valid = 1;  //트랩을 설치해도되는 위치냐
                                                                                //nqueen problem으로 해보기!!!그러나 똑같은 배치만 나올 수도 있다, 리커젼 풀리는 순서가 같기때문
                if (((x == 0) && (y == 0)) || ((x==mapsize-1)&&(y==mapsize-1)))   //그럼 예외를 몇 개하고 길 없을때마다 다시 배치하는건?확률을 명확히 따져봐야 할 것,
                    valid = 0;

                if (valid == 1)
                {
                    newhole = Instantiate(hole);
                    newhole.transform.position = new Vector3(script.grid[x, y].worldPosition.x, script.grid[x, y].worldPosition.y, 2);
                    script.grid[x, y].is_trap = true;
                    holes[h] = newhole; // 실제 오브젝트를 따로 배열에 저장함 => 아이템에서 실제 오브젝트를 건드려야해서
                        h++;
                    i++;
                }
            }
        } while (i != hole_cnt);
    }

    public void touchedtrap(){
        //플레이어가 지뢰를 밟았으면 이벤트로 아이콘 변경
        //왜 다른 script에서 변수 받아오는거 update에 넣으면 안되는지 질문하기
        //if(script.grid[script2.player.hear.x,script2.player.hear.y].trap==true)
        //      event~~ or 그림 덧대기
    }





   
}