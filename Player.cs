using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
    /* 플레이어가 구멍과 아이템이랑 만났을때 처리 및 라이프 관리*/
{
    public int maxLife = 3;
    public static int life = 3; // 라이프 3개
    bool gameOver = false;
    List<Vector3> h = new List<Vector3>(); //구멍 위치가 담긴 리스트
    public Transform playerPoint; // 플레이어 위치
    PlayerMove test;

    // Use this for initialization
    void Start()
    {
        test = GameObject.Find("Player").GetComponent<PlayerMove>();
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 0) { // 라이프 0일때
            if (!gameOver) {
                Die();
            }
        }

    }
    private void FixedUpdate()
    {
        if (life == 0)
            return;
    }
    void Die() { // 사망시
        gameOver = true;
        GameOver();
    }
    void GameOver() {
        // 이부분은 게임 매니저와 연결해서 점수 시간 보여주고 매뉴로 빠지도록해야됨       
        SceneManager.LoadScene("Game Over UI'");
    }
    
    private void OnTriggerEnter2D(Collider2D Player)
    {
        ObjectManager o = new ObjectManager();
        Vector2 gridWorldSize = o.getSize();

        if (Player.gameObject.tag == "Hole") // 구멍과 만나버렸다 => 하.. 이거 큰일남 구멍 위치가 0인 것은 
        {
            life -= 1; // 라이프 까짐

      
            playerPoint.position = new Vector2(-(gridWorldSize.x *5) + 5, (gridWorldSize.y *5) - 5); // 귀찮으니 처음 위치로 보냅니다.
            StopCoroutine(test.CoMove());//무브 포인트도 같이 이동해야되니깐 // 이게 지금 안따라오네... 흠....귀찮으니 내일한다 일단
            StartCoroutine(test.CoMove()); //무브포인트 코루틴 시작

            Debug.Log(life);
        }
    }

}
