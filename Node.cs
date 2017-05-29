using UnityEngine;
using System.Collections;

public class Node
{
    public Vector3 worldPosition; // 현재 노드의 좌표
    public bool is_trap = false;
    public bool is_lighthouse = false;
    // public Node parent


    public Node(Vector3 worldPosition)
    {
        this.worldPosition = worldPosition;
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
