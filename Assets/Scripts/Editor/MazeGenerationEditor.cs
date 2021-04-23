using System.Collections;
using System.Collections.Generic;
using MazeGeneration;
using UnityEditor;
using UnityEngine;

public class MazeGenerationEditor
{
    [MenuItem("Mazes/GenerateMaze %g")]
    public static void GenerateMaze()
    {
        var mazeController = Object.FindObjectOfType<MazeController>();
        mazeController.GenerateMaze();
    } 
}
