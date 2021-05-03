﻿using IngameDebugConsole;
using UnityEngine.Tilemaps;

namespace ConsoleCommands
{
    public class PlayerCommands
    {
        [ConsoleMethod("maze.player.god_mode", "Enables or disables god mode")]
        public static void GodMode(bool enable)
        {
            var wallsCollider = GameManager.Instance.Maze.WallSpriteTilemap.GetComponent<TilemapCollider2D>();
            wallsCollider.enabled = !enable;
        }
    }
}