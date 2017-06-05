﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour
{
    public GameObject object_manager;
    ObjectManager_ script;

    int gridSizeX; // 실제 맵 x길이
    int gridSizeY; // 실제 맵 y길이
    public Transform block;

    void CreateGrid()
    {
        script = object_manager.GetComponent<ObjectManager_>();

        gridSizeX = Mathf.RoundToInt(script.gridWorldSize.x) * 10;
        gridSizeY = Mathf.RoundToInt(script.gridWorldSize.y) * 10;
        script.grid = new Node[Mathf.RoundToInt(script.gridWorldSize.x), Mathf.RoundToInt(script.gridWorldSize.y)];
        //Vector3 worldTopLeft = transform.position - (Vector3.right * gridSizeX / 2) + Vector3.forward * gridSizeY / 2;
        //(0,0에서 x크기만큼 빼고 y크기만큼 더했으니 맵의 좌상이 0,0이 된다.)

        for (int i = 0; i < script.gridWorldSize.x; i++)
        {
            for (int j = 0; j < script.gridWorldSize.y; j++)
            {
                // 현재 노드의 좌표 ((맵에서 좌상 =0,0) - 0,5 )
                Vector2 worldPosition = new Vector2(-(gridSizeX / 2) + 5 + 10 * j, (gridSizeY / 2) - 5 - 10 * i);
                script.grid[i, j] = new Node(worldPosition, i, j);

                Transform newBlock = Instantiate(block);
                newBlock.position = new Vector3(script.grid[i, j].worldPosition.x, script.grid[i, j].worldPosition.y);
            }
        }
    }



    void Start()
    {
        CreateGrid();
    }




    // 맵을 배열로 저장
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

    /* X x Y 이차원 배열
     나는 한칸을 1대신 10의 크기로 잡을것이고
     4 x 4 에서는 배열 0,0 ~ 3,3 까지해서 각 블록의 중심을 위치정보로 저장*/

    // public List<Node> path;
    /* void OnDrawGizmos()
      {
          ObjectManager o = new ObjectManager();
          Vector2 gridWorldSize = o.getSize();

          Gizmos.DrawWireCube(transform.position, new Vector3((gridWorldSize.x) * 10, (gridWorldSize.y) * 10, 1));

      }
      */
}






