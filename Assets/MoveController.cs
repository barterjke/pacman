using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;


public class MoveController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public PathNode spawnNode;
    public PathNode currentNode;
    public PathNode nextNode;

    public Vector2 direction = Vector2.zero;
    public Vector2 nextDirection = Vector2.zero;

    public delegate void ChangeDirection();

    public static event ChangeDirection OnChangeDirection;

    public PathNode GetNodeInTheDirection(Vector2 dir)
    {
        foreach (var node in currentNode.nearest)
        {
            Vector2 directionToNode = node.transform.position - transform.position;
            if (directionToNode.Sign() == dir)
            {
                return node;
            }
        }

        return null;
    }

    private void LateUpdate()
    {
        Vector3 move = Time.deltaTime * moveSpeed * direction;
        if (Vector2.Distance(transform.position, currentNode.transform.position) < move.magnitude)
        {
            var currentNodePosition = currentNode.transform.position;
            Vector2 dif = currentNodePosition - transform.position;
            var scale = 1 - dif.magnitude / move.magnitude;
            Vector3 partialMove = scale * Time.deltaTime * moveSpeed * nextDirection;
            transform.position = currentNodePosition + partialMove;
            if (nextDirection == Vector2.zero && direction != Vector2.zero)
            {
                var node = GetNodeInTheDirection(direction);
                if (node)
                {
                    nextDirection = direction;
                    nextNode = node;
                }
            }

            direction = nextDirection;
            nextDirection = Vector2.zero;
            currentNode = nextNode ? nextNode : currentNode;
            nextNode = null;
            OnChangeDirection?.Invoke();
        }
        else
        {
            transform.position += move;
        }
    }
}