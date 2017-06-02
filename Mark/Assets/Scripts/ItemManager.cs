using UnityEngine;
using System.Collections;

/*실제로 아이템 버튼 누를때 호출하는 메소드*/
public class ItemManager : MonoBehaviour
{
    Item i;

    // 버튼 연속으로 눌러버리면 오류남 - 버튼 누름과 동시에 버튼 비활성화로 만들 방법이 없을까?

    public void OBDUse()
    {
        StartCoroutine(i.OBDScript());
        
        StopCoroutine(i.OBDScript());

    }

    public void EBDUse()
    {
        i.EBScript();
    }
    public void KUse()
    {
        StartCoroutine(i.KnightScript());
        StopCoroutine(i.KnightScript());

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
