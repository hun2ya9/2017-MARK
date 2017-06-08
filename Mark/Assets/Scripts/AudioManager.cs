using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public GameObject Mute_On;
    public GameObject Mute_Off;
    /* 배경음악 싸그리 가져와서 enable = false 처리
     효과음 싸그리 가져와서 ''*/


    public void Mute_true() {
        GameObject.FindGameObjectWithTag("OpenBGM").GetComponent<AudioSource>().mute = true;
        Mute_Off.SetActive(false);
        Mute_On.SetActive(true);
        //GameObject.Find("InGameBGM").GetComponent<AudioSource>().mute = true;
    }
    public void Mute_false()
    {
        GameObject.FindGameObjectWithTag("OpenBGM").GetComponent<AudioSource>().mute = false;
        Mute_Off.SetActive(true);
        Mute_On.SetActive(false);
        //GameObject.Find("InGameBGM").GetComponent<AudioSource>().mute = true;
    }

    public void Mute_true2()
    {
        GameObject.FindGameObjectWithTag("InGameBGM").GetComponent<AudioSource>().mute = true;
        Mute_Off.SetActive(false);
        Mute_On.SetActive(true);
    }
    public void Mute_false2()
    {
        GameObject.FindGameObjectWithTag("InGameBGM").GetComponent<AudioSource>().mute = false;
        Mute_Off.SetActive(true);
        Mute_On.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
