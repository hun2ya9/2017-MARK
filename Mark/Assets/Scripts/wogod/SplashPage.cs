using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashPage : MonoBehaviour {
    
    public float waitingSpan;
    public string titleSceneName;

    void Start()
    {
        Invoke("Go", waitingSpan);
    }

    void Go()
    {
        SceneManager.LoadScene("Opening UI");
    }
}
