using IngameDebugConsole;
using MazeGeneration;
using UnityEngine;

namespace Editor.ConsoleCommands
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