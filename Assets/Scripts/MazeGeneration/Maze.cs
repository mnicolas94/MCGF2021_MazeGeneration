using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace MazeGeneration
{
    public class Maze : MonoBehaviour
    {
        public Action eventMazeChanged;
        public Action eventMazePostChanged;
        
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap floorSpriteTilemap;
        [SerializeField] private Tilemap floorObjectsTilemap;
        [SerializeField] private Tilemap wallSpriteTilemap;
        [SerializeField] private Tilemap wallObjectsTilemap;
        
        [SerializeField] private TileBase floorSpriteTile;
        [SerializeField] private TileBase wallSpriteTile;

        public Tilemap FloorSpriteTilemap => floorSpriteTilemap;

        public Tilemap FloorObjectsTilemap => floorObjectsTilemap;

        public Tilemap WallSpriteTilemap => wallSpriteTilemap;

        public Tilemap WallObjectsTilemap => wallObjectsTilemap;

        /// <summary>
        /// Remove tiles of objects outside walls and floor.
        /// </summary>
        [NaughtyAttributes.Button]
        public void CleanExtraObjectTiles()
        {
            CleanExtraObjectTiles(floorObjectsTilemap, floorSpriteTilemap);
            CleanExtraObjectTiles(wallObjectsTilemap, wallSpriteTilemap);
        }

        private void CleanExtraObjectTiles(Tilemap objectTilemap, Tilemap spriteTilemap)
        {
            var tilesPos = objectTilemap.GetTilePositions();
            foreach (var tilepos in tilesPos)
            {
                if (!spriteTilemap.HasTile(tilepos))
                {
                    objectTilemap.SetTile(tilepos, null);
                }
            }
        }

        [NaughtyAttributes.Button]
        public void ClearFloorObjects()
        {
            floorObjectsTilemap.ClearAllTiles();
        }
    
        [NaughtyAttributes.Button]
        public void ClearWallObjects()
        {
            wallObjectsTilemap.ClearAllTiles();
        }
    
        public void ClearMaze()
        {
            floorSpriteTilemap.ClearAllTiles();
            floorObjectsTilemap.ClearAllTiles();
            wallSpriteTilemap.ClearAllTiles();
            wallObjectsTilemap.ClearAllTiles();
        }
        
        public void PopulateWallsAndFloor(
            bool[][] mazeStructure, int width, int height)
        {
            // offsets
            int xoffset = -width / 2;
            int yoffset = -height / 2;
        
            // place walls and floor
            Vector3Int tmpPos = Vector3Int.zero;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tmpPos.x = j + xoffset;
                    tmpPos.y = i + yoffset;
                
                    if (i == 0 || j == 0 || i == height - 1 || j == width - 1)  // edge walls
                        WallSpriteTilemap.SetTile(tmpPos, wallSpriteTile);
                    else
                    {
                        if (mazeStructure[i - 1][j - 1])
                            FloorSpriteTilemap.SetTile(tmpPos, floorSpriteTile);
                        else
                            WallSpriteTilemap.SetTile(tmpPos, wallSpriteTile);       
                    }
                }
            }
        }

    }
}
