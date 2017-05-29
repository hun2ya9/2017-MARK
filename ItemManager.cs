using UnityEngine;
using System.Collections;

/*실제로 아이템 버튼 누를때 호출하는 메소드*/
public class ItemManager : MonoBehaviour
{
    Item i;

    public void OBDUse()
    {
        StartCoroutine(i.OBDScript());
    }

    public void EBDUse()
    {
        i.EBScript();
    }
    public void KUse()
    {
        StartCoroutine(i.KnightScript());
    }
    // Use this for initialization
    void Start()
    {
        i = GameObject.Find("Player").GetComponent<Item>();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
