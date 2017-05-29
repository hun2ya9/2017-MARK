using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager_ : MonoBehaviour
{

    //난이도 조정에 따라 맵에 값을 넘겨줄 슈퍼 클래스
    //난이도 : 1,2,3값 일단 기본값1로 두고 코딩
    public Vector2 gridWorldSize;
    public Node[,] grid;
    public bool exist_root = false;

    public void input_your_level()   //level을 입력받으면 초기화
    {
        //this.level=yourlevel
    }

    public void mapsize()
    {

        //초급
        if (UIManager.level == 1)
        {
            gridWorldSize = new Vector2(5 + UIManager.stage, 5 + UIManager.stage);
        }
        //중급
        if (UIManager.level == 2)
        {
            gridWorldSize = new Vector2(6 + UIManager.stage, 6 + UIManager.stage);
        }
        //고급
        if (UIManager.level == 3)
        {
            gridWorldSize = new Vector2(7 + UIManager.stage, 7 + UIManager.stage);
        }

        // End 지점에 도착했을때 맵크기 +1하는것도 
        //  gridSizeX +=1;
        //    gridSizeY +=1;
        // 이래서 게임 매니저를 만드는구나 싶다...
        // 최상위 슈퍼클래스 GM 를 만들고 거기다 사용할 변수 함수 다 적어넣어야겠네..


    }
    

    void Awake()
    {
        //input_your_level();
        mapsize();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (exist_root == false)
        {
            while (exist_root != true)
            {
                initial_array();
                trap();
                light_house();
                bfs_root();
     
            }
        }*/
    }
}
