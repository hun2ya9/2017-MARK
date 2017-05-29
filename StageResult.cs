using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class StageResult : MonoBehaviour {

    Text D,T,U,S;
    string Difficulty;
    string Time = Timer.time.ToString();
    string UseItem = Item.useItem.ToString();

    void Start()
    {
        D = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<Text>();
        T = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        U = GameObject.FindGameObjectWithTag("UseItem").GetComponent<Text>();
        S = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        if (UIManager.level == 1) {
            Difficulty = "Easy";
        }
        else if (UIManager.level == 2) {
            Difficulty = "Normal";
        }
        else if (UIManager.level == 3) {
            Difficulty = "Hard";
        }
        
        D.text = Difficulty;
        T.text = Time;
        U.text = UseItem;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
