using MazeGeneration;
using UnityEngine;

public class GenerateMazeOnKeyPress : MonoBehaviour
{
    [SerializeField] private MazeController mazeController;
    [SerializeField] private KeyCode key;
        
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            mazeController.GenerateMaze();
        }
    }
}