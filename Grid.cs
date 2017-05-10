using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Grid : MonoBehaviour {
    protected int gridSizeX; // 실제 맵 x길이
    protected int gridSizeY; // 실제 맵 y길이
    public Vector2 gridWorldSize; // 4 x 4 이런식으로
    protected Node[,] grid;
    public Transform block;
    

    void CreateGrid()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x) * 10;
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y) * 10;
        grid = new Node[Mathf.RoundToInt(gridWorldSize.x), Mathf.RoundToInt(gridWorldSize.y)];
        //Vector3 worldTopLeft = transform.position - (Vector3.right * gridSizeX / 2) + Vector3.forward * gridSizeY / 2;
        //(0,0에서 x크기만큼 빼고 y크기만큼 더했으니 맵의 좌상이 0,0이 된다.)

        for (int i = 0; i < gridWorldSize.x; i++)
        {
            for (int j = 0; j < gridWorldSize.y; j++)
            {
                // 현재 노드의 좌표 ((맵에서 좌상 =0,0) - 0,5 )
                Vector3 worldPosition = new Vector3(-(gridSizeX / 2) + 5 + 10 * j, (gridSizeY / 2) - 5 - 10 * i);
                grid[i, j] = new Node(worldPosition, i, j);
                
                            Transform newBlock = Instantiate(block);
                            newBlock.position = new Vector3(grid[i, j].worldPosition.x, grid[i, j].worldPosition.y);
                        
                        }
            
        }
        /* 배열로치면(0,0)(1,0)(2,0)(j,0) 
         *           (0,1)(1,1)...
         *           (0,2)(1,2)
         * 4 x 4맵에서는
         * (0,0)에는 (-15,15)의 위치를 저장한거지
         * 
           (-15,15)  |(-5,15) |(5.15) |(15,15)
         * ----------|--------|-------|-------
           (-15,5)   |(-5,5)  |(5,5)  |(15,5)
           ----------|--------|-------|-------
           (-15,-5)  |(-5,-5) |(5,-5) |(15,-5)
           ----------|--------|-------|-------
           (-15,-15) |(-5,-15)|(5,-15)|(15,-15)
           ----------|--------|-------|-------
           대충 요론식으로 배열에 위치가 저장된다.
     */

    } // 맵을 배열로 저장
    
    /* X x Y 이차원 배열
     나는 한칸을 1대신 10의 크기로 잡을것이고
     4 x 4 에서는 배열 0,0 ~ 3,3 까지해서 각 블록의 중심을 위치정보로 저장*/
    void Start() {
        CreateGrid();

    }

    // public List<Node> path;
   void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3((gridWorldSize.x) * 10, (gridWorldSize.y) * 10, 1));

    }

}



   
    

