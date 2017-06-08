using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lighthouse : MonoBehaviour {

    public GameObject object_manager;
    ObjectManager_ script;
    public GameObject house;
    public GameObject Text;
    Text g;

    // Use this for initialization
    void Start () {
       
        light_house();
        viewlight();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void light_house()
    {
        script = object_manager.GetComponent<ObjectManager_>();

        int i = 0, x = 0, y = 0;
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        int house_cnt = (mapsize / 2) + 1;   //맵 한 줄의 길이/2==설치할 등대의 수 

        do
        {
            x = Random.Range(0, mapsize - 1);
            y = Random.Range(0, mapsize - 1);
            if ((script.grid[x, y].is_trap == false) && (script.grid[x, y].is_lighthouse == false))  //트랩 또는 등대가 이미 설치되있냐
            {
                int valid = 1;  //등대를 설치해도되는 위치냐
                                //nqueen problem으로 해보기!!!그러나 똑같은 배치만 나올 수도 있다, 리커젼 풀리는 순서가 같기때문
                if (((x == 0) && (y == 0)) || ((x == mapsize - 1) && (y == mapsize - 1)))   //그럼 예외를 몇 개하고 길 없을때마다 다시 배치하는건?확률을 명확히 따져봐야 할 것,
                    valid = 0;

                if (valid == 1)
                {
                    GameObject newhouse = Instantiate(house);
                    newhouse.transform.position = new Vector3(script.grid[x, y].worldPosition.x, script.grid[x, y].worldPosition.y, 2);
                    script.grid[x, y].is_lighthouse = true;
                    i++;
                }
            }
        } while (i != house_cnt +1);
    }

    public void viewlight()   //같은 행과 열에 지뢰가 몇 개 있는지
    {
        g = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Text>();
        int mapsize = Mathf.RoundToInt(script.gridWorldSize.x);
        for (int x = 0; x < mapsize; x++)    //등대 설치할 때 일차원배열에 바로 넣어줬으면 좋았으련만 시간이 없으므로 패스
            for (int y = 0; y < mapsize; y++)
                if (script.grid[x, y].is_lighthouse == true)
                {
                    int trap_cnt = 0;
                    for (int i = 0; i < mapsize; i++)
                    {
                        if (script.grid[x, i].is_trap == true)
                            trap_cnt++;
                        if (script.grid[i, y].is_trap == true)
                            trap_cnt++;
                    }
                    g.text = trap_cnt + "";
                    GameObject G = Instantiate(Text);
                    G.transform.position = script.grid[x, y].worldPosition;
                }


    } 
}
