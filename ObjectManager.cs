using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {
   
    //난이도 조정에 따라 맵에 값을 넘겨줄 슈퍼 클래스
    //난이도 : 1,2,3값 일단 기본값1로 두고 코딩

    public ObjectManager(){ // 생성자에서 값을 줘야됨
        c();
    }
        int level = 1;
    public static Vector2 gridWorldSize;
    public void c() {
        //초급
        if (level == 1) {
            gridWorldSize = new Vector2(6,6);
        }
        //중급
        if (level == 2) {
            gridWorldSize = new Vector2(7,7);
        }
        //고급
        if (level == 3) {
            gridWorldSize = new Vector2(8,8);
        }

        // End 지점에 도착했을때 맵크기 +1하는것도 
        //  gridSizeX +=1;
        //    gridSizeY +=1;
        // 이래서 게임 매니저를 만드는구나 싶다...
        // 최상위 슈퍼클래스 GM 를 만들고 거기다 사용할 변수 함수 다 적어넣어야겠네..

    
    }
    public Vector2 getSize()
    {
        return gridWorldSize;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
