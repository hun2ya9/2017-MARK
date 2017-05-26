using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* 도착 위치에 도착했을시
 플레이어 위치*/

public class EndPoint : MonoBehaviour
{

    public Transform playerPoint; // 플레이어 위치
    Vector2 EndPosition; // 도착지점

    void end() {


        if (playerPoint.position.x == EndPosition.x && playerPoint.position.y == EndPosition.y) {        
            SceneManager.LoadScene("Stage Result UI");

        }
    }
    
    
    // Use this for initialization
    void Start()
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();
        EndPosition = new Vector2(gridWorldSize.x * 5 - 5, (-gridWorldSize.y * 5) + 5);       
    }

    // Update is called once per frame
    void Update()
    {
        end();
    }
}
