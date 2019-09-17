using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Public 
    public Transform startPosition;
    public LayerMask wallMask;
    public Vector2 gridSize;
    public float nodeRadiusSize;
    public float distanceBetweenNode;

    public List<NodeClass> finalPath;
    
    //Private
    private NodeClass[,] _nodesClass;
    private int xNodes;
    private int yNodes;
    private float nodeDiameterSize;
    
    private GameObject mapGame;
    private float distance;

    
    void Start()
    {
        mapGame = GameObject.FindWithTag("mapGame");
        this.transform.position = mapGame.transform.position;
        gridSize.x = mapGame.transform.localScale.x * 10;
        gridSize.y = mapGame.transform.localScale.z * 10;
        nodeDiameterSize = nodeRadiusSize * 2;
        xNodes = Mathf.RoundToInt(gridSize.x / nodeDiameterSize);
        yNodes = Mathf.RoundToInt(gridSize.y / nodeDiameterSize);
        CreateGrid();
    }

    private void LateUpdate()
    {
        if(distanceBetweenNode != distance)
        {
            CreateGrid();
        }
        distance = distanceBetweenNode;
    }

    void CreateGrid()
    {
        _nodesClass = new NodeClass[xNodes, yNodes];
        Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        Vector3 upperLeft = transform.position + Vector3.left * gridSize.x / 2 + Vector3.forward * gridSize.y / 2 * -1;
        Vector3 bottomRight = transform.position - Vector3.left * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        Vector3 upperRight = transform.position - Vector3.left * gridSize.x / 2 + Vector3.forward * gridSize.y / 2 ;


        for (int x = 0; x < xNodes; x++)
        {
            for (int y = 0; y < yNodes; y++)
            {
                //To create nodes from left upper/bottom to right 
                // Vector3 worldPoint = bottomLeft/upperLeft + Vector3.right * (x * nodeDiameterSize + nodeRadiusSize) + Vector3.forward * (y * nodeDiameterSize + nodeRadiusSize);

                //If you want create nodes from bottomRight to left 
                // Vector3 worldPoint = bottomRight - Vector3.right * (x * nodeDiameterSize + nodeRadiusSize) + Vector3.forward * (y * nodeDiameterSize + nodeRadiusSize);
                
                //Else if you want create nodes from upperRight to left 
                // Vector3 worldPoint = upperRight - Vector3.right * (x * nodeDiameterSize + nodeRadiusSize) - Vector3.forward * (y * nodeDiameterSize + nodeRadiusSize);

                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameterSize + nodeRadiusSize) + Vector3.forward * (y * nodeDiameterSize + nodeRadiusSize);

                bool wall = !Physics.CheckSphere(worldPoint, nodeRadiusSize, wallMask);
                
                _nodesClass[x,y] = new NodeClass(wall, worldPoint, x, y);
            }            
        }
    }
    
    public List<NodeClass> GetNeighbourNodes(NodeClass neighbourNodeClass)
    {
        List<NodeClass> neighbourList = new List<NodeClass>();
        int checkX;
        int checkY;

        //Check the right side of the current node.
        checkX = neighbourNodeClass.xGridPos + 1;
        checkY = neighbourNodeClass.yGridPos;
        if (checkX >= 0 && checkX < xNodes)//If the XPosition is in range of the array
        {
            if (checkY >= 0 && checkY < yNodes)//If the YPosition is in range of the array
            {
                neighbourList.Add(_nodesClass[checkX, checkY]);
            }
        }
        
        //Check the Left side of the current node.
        checkX = neighbourNodeClass.xGridPos - 1;
        checkY = neighbourNodeClass.yGridPos;
        if (checkX >= 0 && checkX < xNodes)
        {
            if (checkY >= 0 && checkY < yNodes)
            {
                neighbourList.Add(_nodesClass[checkX, checkY]);
            }
        }
        
        //Check the Top side of the current node.
        checkX = neighbourNodeClass.xGridPos;
        checkY = neighbourNodeClass.yGridPos + 1;
        if (checkX >= 0 && checkX < xNodes)
        {
            if (checkY >= 0 && checkY < yNodes)
            {
                neighbourList.Add(_nodesClass[checkX, checkY]);
            }
        }
        
        //Check the Bottom side of the current node.
        checkX = neighbourNodeClass.xGridPos;
        checkY = neighbourNodeClass.yGridPos - 1;
        if (checkX >= 0 && checkX < xNodes)
        {
            if (checkY >= 0 && checkY < yNodes)
            {
                neighbourList.Add(_nodesClass[checkX, checkY]);
            }
        }

        return neighbourList;//Return the neighbours list.
    }

    //Gets the closest node to the given world position.
    public NodeClass NodeFromWorldPoint(Vector3 _worldPos)
    {
        float xPos = ((_worldPos.x + gridSize.x / 2) / gridSize.x);
        float yPos = ((_worldPos.z + gridSize.y / 2) / gridSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((xNodes - 1) * xPos);
        int y = Mathf.RoundToInt((yNodes - 1) * yPos);

        return _nodesClass[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (_nodesClass != null)
        {
            foreach (NodeClass n in _nodesClass)
            {
                if (n.isWall)
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.yellow;//Set the color of the wall
                }


                if (finalPath != null)
                {
                    if (finalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;//Set the color of the path node
                    }

                }


                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameterSize - distanceBetweenNode));//Draw the node at the position of the node.
            }
        }
    }
}
