using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public List<PathNode> nearest = new();

    void Start()
    {
        foreach (var pathNode in nearest)
        {
            if (pathNode == null) continue;
            if (!pathNode.nearest.Contains(this))
            {
                pathNode.nearest.Add(this);
            }
        }
    }

    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        foreach (var pathNode in nearest)
        {
            if (pathNode == null) continue;
            Gizmos.DrawLine(transform.position, pathNode.transform.position);
        }
    }
}