using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private MoveController _moveController;
    public float scoreCounter;
    public uint hp = 3;
    public Animator animator;
    private List<Collectable> _scores;
    public bool freeze;
    private const float InteractionDistance = 0.1f;
    private List<GhostAI> ghosts;


    private void OnChangeAnimation()
    {
        var direction = _moveController.direction;
        float angle = 0;
        if (direction == Vector2.zero)
        {
            return;
        }

        if (direction == Vector2.right)
        {
            angle = 0;
        }

        if (direction == Vector2.down)
        {
            angle = -90;
        }
        else if (direction == Vector2.up)
        {
            angle = 90;
        }

        else if (direction == Vector2.left)
        {
            angle = 180;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Start()
    {
        ghosts = Resources.FindObjectsOfTypeAll<GhostAI>().ToList();
        animator = GetComponent<Animator>();
        _moveController = GetComponent<MoveController>();
        MoveController.OnChangeDirection += OnChangeAnimation;
        _scores = Resources.FindObjectsOfTypeAll<Collectable>().ToList();
        ghosts = Resources.FindObjectsOfTypeAll<GhostAI>().ToList();
        Reset();
    }

    public void Reset()
    {
        freeze = false;
        _moveController.transform.position = _moveController.spawnNode.transform.position;
        _moveController.currentNode = _moveController.spawnNode;
    }

    private void CheckCollectable()
    {
        foreach (var score in _scores)
        {
            if (Vector2.Distance(transform.position, score.transform.position) > InteractionDistance) continue;
            if (score.type == CollectableType.Score)
            {
                scoreCounter++;
                Destroy(score.gameObject);
                _scores.Remove(score);
                break;
            }

            if (score.type == CollectableType.Energizer)
            {
                scoreCounter++;
                Destroy(score.gameObject);
                _scores.Remove(score);
                foreach (var ghost in ghosts)
                {
                    ghost.ApplyEnergizer();
                }

                break;
            }
        }
    }

    private void CheckEnemyCollision()
    {
        foreach (var ghost in ghosts)
        {
            if (Vector2.Distance(transform.position, ghost.transform.position) > InteractionDistance) continue;
            if (ghost.vulnerable)
            {
                ghost.Reset();
                scoreCounter += 200;
            }
            else
            {
                animator.SetBool("is_dead", true);
                freeze = true;
                _moveController.direction = Vector2.zero;
                _moveController.nextDirection = Vector2.zero;
                // _moveController.currentNode = null;
                _moveController.nextNode = null;
                //ResetLevel
                hp--;
            }

            break;
        }
    }


    void Update()
    {
        if (freeze) return;
        var direction = _moveController.direction;
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).Sign();
        var currentNode = _moveController.currentNode;
        if (input != Vector2.zero)
        {
            foreach (var pathNode in currentNode.nearest)
            {
                Vector2 directionToNode = pathNode.transform.position - currentNode.transform.position;
                if (directionToNode.Sign() == input)
                {
                    if (input == -direction || direction == Vector2.zero)
                    {
                        _moveController.direction = input;
                        _moveController.currentNode = pathNode;
                        _moveController.nextDirection = Vector2.zero;
                        _moveController.nextNode = null;
                        OnChangeAnimation();
                    }
                    else if (input != direction)
                    {
                        _moveController.nextDirection = input;
                        _moveController.nextNode = pathNode;
                    }

                    break;
                }
            }
        }

        CheckCollectable();
        CheckEnemyCollision();
    }
}