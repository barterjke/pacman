using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSpawner : MonoBehaviour
{
    public GameObject score;
    public float frequency = 0.3f;

    private void Awake()
    {
        // return;
        foreach (Transform child in GameObject.Find("Path").transform)
        {
            var childPosition = child.transform.position;
            foreach (var nearNode in child.GetComponent<PathNode>().nearest)
            {
                var counter = 0;
                var spawnPosition = childPosition;
                var step = (nearNode.transform.position - childPosition).Sign() * frequency;
                // if (step.x != 0 && step.y != 0)
                // {
                //     print($"hello {nearNode.gameObject.name} {child.name} {step} {nearNode.transform.position} {childPosition} {(nearNode.transform.position - childPosition).x}");
                // }
                while (Vector2.Distance(spawnPosition , nearNode.transform.position) >= step.magnitude && counter < 10)
                {
                    Instantiate(score, spawnPosition, Quaternion.identity).transform.parent = transform;
                    spawnPosition += step;
                    counter++;
                }
            }
        }
    }

    void Update()
    {
    }
}