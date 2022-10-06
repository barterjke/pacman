using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private UserInput _userInput;
    private List<GhostAI> _ghosts;

    void Start()
    {
        _userInput = FindObjectOfType<UserInput>();
        _ghosts = Resources.FindObjectsOfTypeAll<GhostAI>().ToList();
    }

    public void Reset()
    {
        _userInput.Reset();
        foreach (var ghost in _ghosts)
        {
            ghost.Reset();
        }
        print("reseted");
    }
}