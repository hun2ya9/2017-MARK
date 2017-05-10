using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
        public Vector3 worldPosition; // 현재 노드의 좌표
        public int gridX, gridY; // X, Y 좌표
        public Node parent;


        public Node(Vector3 worldPosition, int gridX, int gridY)
        {
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
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
