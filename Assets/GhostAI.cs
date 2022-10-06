using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostAI : MonoBehaviour
{
    private MoveController _moveController;
    public bool vulnerable;
    private Animator _animator;

    void Start()
    {
        // Random.InitState(2);
        _animator = GetComponent<Animator>();
        _moveController = GetComponent<MoveController>();
        MoveController.OnChangeDirection += OnChangeDirection;
        Reset();
    }

    public void Reset()
    {
        vulnerable = false;
        transform.position = _moveController.spawnNode.transform.position;
        _moveController.currentNode = _moveController.spawnNode;
        var possibleNextNodes = _moveController.currentNode.nearest;
        var nextNode = GetRandomNode(possibleNextNodes);
        _moveController.direction =
            (nextNode.transform.position - _moveController.currentNode.transform.position).Sign();
        _moveController.currentNode = nextNode;
        _moveController.nextNode = null;
        _moveController.nextDirection = Vector2.zero;
    }

    private void DisableEnergizer()
    {
        vulnerable = false;
        // reset animation
    }

    public void ApplyEnergizer()
    {
        vulnerable = true;
        // apply animation
        // change direction
        _moveController.direction = -_moveController.direction;
        Invoke(nameof(DisableEnergizer), 10f);
        // called once in a while, so it's okay and not really expensive
    }

    void UpdateAnimation()
    {
        var dir = 0;
        if (_moveController.direction == Vector2.left)
        {
            dir = 1;
        }

        if (_moveController.direction == Vector2.right)
        {
            dir = 2;
        }

        if (_moveController.direction == Vector2.up)
        {
            dir = 3;
        }

        _animator.SetInteger("direction", dir);
    }

    void OnChangeDirection()
    {
        PathNode nextNode;
        var nearest = _moveController.currentNode.nearest;
        if (nearest.Count == 1)
        {
            nextNode = nearest[0];
        }
        else
        {
            var possibleNextNodes =
                _moveController.currentNode.nearest.Where(it =>
                    it != _moveController.GetNodeInTheDirection(-_moveController.direction)).ToList();
            nextNode = GetRandomNode(possibleNextNodes);
        }

        _moveController.nextDirection =
            (nextNode.transform.position - _moveController.currentNode.transform.position).Sign();
        _moveController.nextNode = nextNode;
        UpdateAnimation();
    }


    PathNode GetRandomNode(List<PathNode> list)
    {
        return list[Random.Range(0, list.Count)];
    }


    // void Update()
    // {
    //     if (_moveController.nextDirection == Vector2.zero)
    //     {
    //     }
    // }
}