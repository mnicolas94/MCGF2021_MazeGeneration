using MazeGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MazeGenerationEditor
    {
        [MenuItem("Mazes/GenerateMaze %g")]
        public static void GenerateMaze()
        {
            var mazeController = Object.FindObjectOfType<MazeController>();
            mazeController.GenerateMaze();
        } 
    }
}
