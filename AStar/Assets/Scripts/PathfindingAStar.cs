using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathfindingAStar : MonoBehaviour
{
    private GridManager gridReference;
    public Transform startPosition;
    public Transform targetPosition;


    private void Awake()
    {
        gridReference = GetComponent<GridManager>();
    }


    void Update()
    {
         FindPath(startPosition.position, targetPosition.position);      
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        NodeClass startNodeClass = gridReference.NodeFromWorldPoint(startPos);
        NodeClass targetNodeClass = gridReference.NodeFromWorldPoint(targetPos);
        
        List<NodeClass> uncheckedNodes = new List<NodeClass>();
        HashSet<NodeClass> checkedNodes = new HashSet<NodeClass>();
        
        uncheckedNodes.Add(startNodeClass);

        while (uncheckedNodes.Count > 0)
        {
            NodeClass currentNodeClass = uncheckedNodes[0];
            for (int i = 1; i < uncheckedNodes.Count; i++)
            {
                if (uncheckedNodes[i].FCost < currentNodeClass.FCost || uncheckedNodes[i].FCost == currentNodeClass.FCost && uncheckedNodes[i].hCost < currentNodeClass.hCost)
                {
                    currentNodeClass = uncheckedNodes[i];
                }
            }

            uncheckedNodes.Remove(currentNodeClass);
            checkedNodes.Add(currentNodeClass);

            if (currentNodeClass == targetNodeClass)
            {
                GetFinalPath(startNodeClass, targetNodeClass);
            }
            
            foreach (NodeClass neighbouringNode in gridReference.GetNeighbourNodes(currentNodeClass)) //Loop every neighbour of te current node
            {
                if (!neighbouringNode.isWall || checkedNodes.Contains(neighbouringNode)) //check if this node is a wall or is already checked
                {
                    continue;
                }

                int moveFCost = currentNodeClass.gCost + DistanceBetweenTwoPoints(currentNodeClass, neighbouringNode);
                if (moveFCost < neighbouringNode.gCost || !uncheckedNodes.Contains(neighbouringNode))
                {
                    neighbouringNode.gCost = moveFCost;
                    neighbouringNode.hCost = DistanceBetweenTwoPoints(neighbouringNode, targetNodeClass);
                    neighbouringNode.ParentNodeClass = currentNodeClass;

                    if (!uncheckedNodes.Contains(neighbouringNode))
                    {
                        uncheckedNodes.Add(neighbouringNode);
                    }
                }
            }
        }
        
     
    }
    
    //This is the ManhattanDistance formula that used in the most case of AStar Pathfinding
    private int GetManhattanDistance(NodeClass nodeClassA, NodeClass nodeClassB)
    {
        int ix = Mathf.Abs(nodeClassA.xGridPos - nodeClassB.xGridPos);//x1-x2
        int iy = Mathf.Abs(nodeClassA.yGridPos - nodeClassB.yGridPos);//y1-y2

        return ix + iy;//Return the sum
    }

    //This is the Distance Formula & Pythagorean Theroem to calcolate the distance between two points
    private int DistanceBetweenTwoPoints(NodeClass a, NodeClass b)
    {
        float xPow = Mathf.Pow(b.xGridPos-a.xGridPos,2);
        float yPow = Mathf.Pow(b.yGridPos-a.yGridPos,2);

        float xSqrt = Mathf.Sqrt(xPow);
        float ySqrt = Mathf.Sqrt(yPow);

        int xResult = (int) xSqrt;
        int yResult = (int) ySqrt;

        return Mathf.Abs(xResult + yResult);
    }
    
    void GetFinalPath(NodeClass startNodeClass, NodeClass targetNodeClass)
    {
        List<NodeClass> finalPath = new List<NodeClass>();
        NodeClass currentNodeClass = targetNodeClass;

        while(currentNodeClass != startNodeClass)
        {
            finalPath.Add(currentNodeClass);
            currentNodeClass = currentNodeClass.ParentNodeClass;
        }

        finalPath.Reverse();//Reverse the path to get the correct order

        gridReference.finalPath = finalPath;//Set the final path
    }
}
