using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMStram : MonoBehaviour
{ 
    //public static BGMStram instance = null;

    //public static GameObject sound;
    //AudioClip s;
    void Start()
    {       
    }
    
    void Awake()
    {
        //s = sound.GetComponent<AudioClip>();
        DontDestroyOnLoad(transform.gameObject);

        /*
                if (instance == null)
                    instance = this;
                else if (instance != this)
                   Destroy(sound);

          */
    }

}
