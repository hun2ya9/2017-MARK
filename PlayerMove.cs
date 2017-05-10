using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Grid
{

    public Transform playerPoint;

    void Start()
    {
        //Vector2 worldTopLeft = Vector3.zero - (Vector3.right * gridSizeX / 2) + Vector3.forward * gridSizeY / 2;
        playerPoint.position = new Vector2(-(gridWorldSize.x * 5) + 5, (gridWorldSize.y * 5) - 5);
    }
    void Update()
    {
        move();
    }  // 마우스로 이동

    void move() { 
        if (Input.GetMouseButtonDown(0))
        { // 클릭시 좌표 저장
          //Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          /*position을 화면 공간에서 월드 공간으로 변경시킵니다.
          화면 공간은 픽셀로 정의됩니다. 화면의 좌하단은(0,0)이며; 
          우상단은(pixelWidth, pixelHeight)입니다.
          z position은 카메라로부터의 거리를 월드 단위로 환산한 값입니다. 
  */
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    /*범위 안에 들어왔을때 해당 칸의 중심으로 이동하게끔
    오른쪽 : -15 < x < -5 , -5 < y < 5 v
    왼쪽   :  5 < x < 15 , -5 < y < 5
    위     : -5 < x < 5, -15 < y < -5         
    아래   : -5 < x < 5, 5 < y < 15        
    대각선 
    우하   : -15 < x < -5 , 5 < y < 15 v
    좌하   :  5 < x < 15  , 5 < y < 15
    우상   : -15 < x < -5 , -15 < y < -5 v
    좌상   : 5 < x < 15 , -15 < y < -5
     * 
     */
    Vector3 a;
    a.x = playerPoint.position.x - pos.x;
            a.y = playerPoint.position.y - pos.y;

            if ((a.x > -15 && a.x< -5) && (a.y > -15 && a.y< -5))
            { // 우상
                playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y + 10);
}
            else if ((a.x > -15 && a.x< -5) && (a.y > 5 && a.y< 15))
            { // 우하
                playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y - 10);
            }
            else if ((a.x > -15 && a.x< -5) && (a.y > -5 && a.y< 5))
            { // 우
                playerPoint.position = new Vector2(playerPoint.position.x + 10, playerPoint.position.y);
            }
            else if ((a.x > -5 && a.x< 5) && (a.y > -15 && a.y< -5))
            { // 위
                playerPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y + 10);
            }
            else if ((a.x > -5 && a.x< 5) && (a.y > 5 && a.y< 15))
            {// 아래
                playerPoint.position = new Vector2(playerPoint.position.x, playerPoint.position.y - 10);
            }
            else if ((a.x > 5 && a.x< 15) && (a.y > -15 && a.y< -5))
            { // 좌상 
                playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y + 10);
            }
            else if ((a.x > 5 && a.x< 15) && (a.y > -5 && a.y< 5))
            { // 왼
                playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y);
            }
            else if ((a.x > 5 && a.x< 15) && (a.y > 5 && a.y< 15))
            { //좌하
                playerPoint.position = new Vector2(playerPoint.position.x - 10, playerPoint.position.y - 10);
            }
            else
            {
            }
            Debug.Log(playerPoint.position);

        }
    }  


}




        /*
                  Ray2D ray = new Ray2D(pos, Vector2.zero); // ray.origin, ray.direction
                  RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);//(pos, Vector2.zero);

                //&& hit.collider.gameObject.layer == LayerMask.NameToLayer("MovePoint"));
                if (hit.collider != null)
                  {
                    //   if (hit.collider.tag == "MovePoint")
                    //{
                    playerPoint.position = pos;
                    Debug.Log("체크됨");
                      Debug.Log(playerPoint.position);
                      Debug.Log(hit.point);
                      //   }
                  }
                  else
                  {
                      Debug.Log("체크 안됨");
                    Debug.Log(hit.collider);
                    Debug.Log(playerPoint.position);
                     Debug.Log(hit.point);

         



    
                    //Vector2 p = hit.point;
                   // Debug.Log(p);
                
                

            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                playerPoint.position = hit.point;
                Debug.Log(hit.collider.gameObject.name);
                Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
                
                Debug.Log("체크됨");
               
            }
        }
    }
     */ // 실패방법



    