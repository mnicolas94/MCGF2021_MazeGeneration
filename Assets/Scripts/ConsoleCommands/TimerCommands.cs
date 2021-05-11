using System;
using Character;
using IngameDebugConsole;
using Timer;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace ConsoleCommands
{
    public class TimerCommands
    {
        [ConsoleMethod("maze.timer.reset", "Resets the maze timer")]
        public static void ResetTimer(bool enable)
        {
            var timer = Object.FindObjectOfType<MazeGenerationTimer>();
            timer.ResetTimer();
        }
        
        [ConsoleMethod("maze.timer.stop", "Resets the maze timer")]
        public static void StopTimer(bool enable)
        {
            var timer = Object.FindObjectOfType<MazeGenerationTimer>();
            timer.StopTimer();
        }
    }
}