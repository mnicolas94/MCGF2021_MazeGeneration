using System;
using MazeGeneration;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    [SerializeField] private MazeController mazecontroller;
    
    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void NotifyPuzzleSolved()
    {
        mazecontroller.GenerateMaze();
    }
}