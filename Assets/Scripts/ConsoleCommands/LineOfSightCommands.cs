using IngameDebugConsole;
using UnityEngine;

namespace ConsoleCommands
{
    public class LineOfSightCommands
    {
        [ConsoleMethod("maze.los_radius", "Log the line of sight radius")]
        public static void GetLineOfSightRadius()
        {
            var lineOfSightData = GameManager.Instance.LineOfSightData;
            Debug.Log($"LoS = {lineOfSightData.LineOfSightRadius}");
        }
        
        [ConsoleMethod("maze.los_radius", "Set the line of sight radius")]
        public static void GetLineOfSightRadius(float los)
        {
            var lineOfSightData = GameManager.Instance.LineOfSightData;
            lineOfSightData.SetLineOfSightRadius(los);
        }
    }
}