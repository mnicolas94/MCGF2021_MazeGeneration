using IngameDebugConsole;
using MazeGeneration;
using UnityEngine;

namespace ConsoleCommands
{
    public class MazeGenerationCommands
    {
        [ConsoleMethod("maze.gen_maze", "Generates a maze")]
        public static void GenerateMaze()
        {
            var mazeController = Object.FindObjectOfType<MazeController>();
            mazeController.GenerateMaze();
        }
    }
}