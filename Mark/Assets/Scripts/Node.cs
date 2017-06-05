using UnityEngine;
using System.Collections;

public class Node
{
    public Vector3 worldPosition; // 현재 노드의 좌표
    public bool is_trap = false;
    public bool is_lighthouse = false;
    public bool is_Item = false;
    public bool is_flag = false;
    public int X, Y;
    public int bfs_visit = 0;
    public int bfs_distance = 0;
    // public Node parent


    public Node(Vector3 worldPosition, int i, int j)
    {
        this.worldPosition = worldPosition;
        this.X = i;
        this.Y = j;
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
