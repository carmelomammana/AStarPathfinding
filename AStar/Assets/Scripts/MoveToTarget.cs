using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    
    public GridManager grid;

    private bool goNext;
    private int index;
    void Start()
    {
        index = 0;
        goNext = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (goNext && grid.finalPath != null)
        {
            goNext = false;
            if (GetNextNode(index) != null)
            {
                this.transform.position = GetNextNode(index).worldPosition;
                StartCoroutine(Move());
            }
        }
    }

    private NodeClass GetNextNode(int i)
    {
        if (i > grid.finalPath.Count)
        {
            return null;
        }
        else
        {
            return grid.finalPath[i];
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);
        goNext = true;
    }
}
