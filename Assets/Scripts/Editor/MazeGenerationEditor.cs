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
        
        [MenuItem("Mazes/GenerateMazeWithPuzzles #&g")]
        public static void GenerateMazeWithPuzzles()
        {
            var gameManager = Object.FindObjectOfType<GameManager>();
            gameManager.GenerateMazeWithNewPuzzles();
        }
    }
     
}
