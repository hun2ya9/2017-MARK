using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static float time; // 시간
    Text t;
    // Use this for initialization
    void Start()
    {
        t = GetComponent<Text>();
        time = 0;

    }

    // Update is called once per frame
    void Update() // 실행시 tt는 계속해서 1씩 더해감
    {
        time += Time.deltaTime;
        int tt = Mathf.FloorToInt(time);
        float minute = Mathf.FloorToInt(tt / 60);
        string second = tt.ToString();
        string ResetSecond = (tt - 60* Mathf.FloorToInt(tt / 60)).ToString();

        if (tt < 60)
        {
            t.text = "Time : 00분" + second + "초";
        }
        else if (tt >= 60)
        {
            t.text = "Time :" + minute + "분" + ResetSecond + "초";
        }
    }

   
     
}
