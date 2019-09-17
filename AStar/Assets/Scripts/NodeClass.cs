using UnityEngine;

public class NodeClass
{
    public int xGridPos;
    public int yGridPos;
    
    public bool isWall;
    
    public Vector3 worldPosition;
    public NodeClass ParentNodeClass;
    
    public int gCost;
    public int hCost;
    
    public int FCost
    {
        get { return gCost + hCost; }
    }

    public NodeClass(bool _isWall, Vector3 _worldPosition, int _xGridPos, int _yGridPos)
    {
        isWall = _isWall;
        worldPosition = _worldPosition;
        xGridPos = _xGridPos;
        yGridPos = _yGridPos;        
    }
}