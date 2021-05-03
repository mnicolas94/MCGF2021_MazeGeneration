using System.Runtime.InteropServices;
using Character;
using IngameDebugConsole;
using UnityEditor;
using UnityEngine;

namespace Editor.ConsoleCommands
{
    public class LineOfSightCommands
    {
        [ConsoleMethod("maze.los_radius", "Log the line of sight radius")]
        public static void GetLineOfSightRadius()
        {
            var lineOfSightData = AssetDatabase.LoadAssetAtPath<LineOfSightData>("Assets/Data/LineOfSightData.asset");
            Debug.Log($"LoS = {lineOfSightData.LineOfSightRadius}");
        }
        
        [ConsoleMethod("maze.los_radius", "Set the line of sight radius")]
        public static void GetLineOfSightRadius(float los)
        {
            var lineOfSightData = AssetDatabase.LoadAssetAtPath<LineOfSightData>("Assets/Data/LineOfSightData.asset");
            lineOfSightData.SetLineOfSightRadius(los);
        }
    }
}