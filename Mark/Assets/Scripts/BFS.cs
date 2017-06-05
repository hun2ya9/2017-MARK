using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour {
    public GameObject object_manager;
    ObjectManager_ script;

    int[,] offset = new int[8, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, -1 }, { -1, 1 }, { 1, 1 }, { -1, -1 } };    //8방위를 저장한 배열
    Node[] queue = new Node[10000];
    int q_index = 0;
   
    public int is_root_interface()
    {
        script = object_manager.GetComponent<ObjectManager_>();
        
        int lastx, lasty;  //도착지점의 좌표와 큐의 새로운 값을 받을때 들어가는 인덱스
        script.grid[0, 0].bfs_distance = 1;

        lastx = Mathf.RoundToInt(script.gridWorldSize.x)- 1;
        lasty = Mathf.RoundToInt(script.gridWorldSize.y) - 1;
        return bfs_(script.grid[0, 0], lastx, lasty);
    }
    // Use this for initialization
    
    public int bfs_(Node nd,int lastx,int lasty)
    {
        if (nd.X == lastx && nd.Y == lasty)     //현재노드가 도착지점이면 true를 리턴 
        {
            return 1;
        }
        else
        {                         //도착지점이 아니라면
            int d = nd.bfs_distance;
            nd.bfs_visit = 1;
            for (int i = 0; i < 8; i++)         //현재노드의 8방위를 탐색하며 이동할 수 있는 노드는 큐에 넣는다.(조건문에서 'x와y의 좌표가 배열의 크기를 벗어나지 않고 방문한 적이 없고 벽이 아닌 길인 경우에만' 으로 걸러준다)
                if (((0 <= nd.X + offset[i,0]) && (nd.X + offset[i,0] <= lastx)) && ((0 <= nd.Y + offset[i,1]) && (nd.Y + offset[i,1] <= lasty)) && (script.grid[nd.X + offset[i, 0], nd.Y + offset[i, 1]].is_lighthouse == false) && (script.grid[nd.X + offset[i,0],nd.Y + offset[i,1]].is_trap == false) && (script.grid[nd.X + offset[i,0],nd.Y + offset[i,1]].bfs_visit == 0))
                {
                    int q_flag = 0;
                    for (int j = 0; j < q_index; j++)
                        if ((queue[j].X == nd.X+ offset[i,0]) && (queue[j].Y == nd.Y + offset[i,1]))  //큐에 중복된거 있는지
                            q_flag = 1;
                    if (q_flag == 0)
                    {
                        queue[q_index++] = script.grid[nd.X + offset[i,0],nd.Y+ offset[i,1]];
                        script.grid[nd.X + offset[i,0],nd.Y + offset[i,1]].bfs_distance = d + 1;     //호출할 때 거리를 증가시켜주면 2,3,4 동시에 큐에 들어오고 2다음 3호출되는데 거리 같아야하는데 증가되서 되니 안된다
                    }                                                                 //즉, 담을 때 그 노드에 있는 거리값을 올려줘야함.
                }
            while (queue[0] != null)
            {   //큐가 비지 않은 동안 반복
                Node q_zero = queue[0];    //큐의 첫 번쨰 노드를 저장(가장 먼저 들어왔던 노드)
                int k = 0;
                while (queue[k] != null)
                {    //dequeu과정. 노드를 큐에서 하나 뽑았으니 큐 배열을 한 칸씩 앞당겨준다
                    queue[k] = queue[k + 1];
                    k++;
                }
                q_index--;    //노드를 큐에서 하나 뽑았으니 인덱스의 값이 1 줄어든다
                if (bfs_(q_zero,lastx,lasty)==1)
                {    //큐에서 뽑은 노드를 bfs로 재귀호출하고 1이 리턴되면 1을 리턴한다
                    return 1;
                }
            }
            return 0;   //모든 과정을 지나왔는데도 1로 리턴되지 않은 경우 0을 리턴(길이 없는 경우)
        }
    }

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
