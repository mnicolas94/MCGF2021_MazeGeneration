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

        public BoundsInt GetBounds()
        {
            var floorSpriteBounds = floorSpriteTilemap.RealCellBounds();
            var floorObjectsBounds = floorObjectsTilemap.RealCellBounds();
            var wallSpriteBounds = wallSpriteTilemap.RealCellBounds();
            var wallObjectsBounds = wallObjectsTilemap.RealCellBounds();
            int xMin = Math.Min(
                Math.Min(floorSpriteBounds.xMin, floorObjectsBounds.xMin),
                Math.Min(wallSpriteBounds.xMin, wallObjectsBounds.xMin));
            int xMax = Math.Max(
                Math.Max(floorSpriteBounds.xMax, floorObjectsBounds.xMax),
                Math.Max(wallSpriteBounds.xMax, wallObjectsBounds.xMax));
            
            int yMin = Math.Min(
                Math.Min(floorSpriteBounds.yMin, floorObjectsBounds.yMin),
                Math.Min(wallSpriteBounds.yMin, wallObjectsBounds.yMin));
            int yMax = Math.Max(
                Math.Max(floorSpriteBounds.yMax, floorObjectsBounds.yMax),
                Math.Max(wallSpriteBounds.yMax, wallObjectsBounds.yMax));

            BoundsInt bounds = new BoundsInt(xMin, yMin, 0, xMax - xMin, yMax - yMin, 1);
            return bounds;
        }
        
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

        [NaughtyAttributes.Button]
        public void Center()
        {
            floorSpriteTilemap.Center();
            floorObjectsTilemap.Center();
            wallSpriteTilemap.Center();
            wallObjectsTilemap.Center();
        }
        
        [NaughtyAttributes.Button]
        public void Rotate90()
        {
            floorSpriteTilemap.Rotate90();
            floorObjectsTilemap.Rotate90();
            wallSpriteTilemap.Rotate90();
            wallObjectsTilemap.Rotate90();
        }
        
        [NaughtyAttributes.Button]
        public void Rotate180()
        {
            floorSpriteTilemap.Rotate180();
            floorObjectsTilemap.Rotate180();
            wallSpriteTilemap.Rotate180();
            wallObjectsTilemap.Rotate180();
        }
        
        [NaughtyAttributes.Button]
        public void Rotate270()
        {
            floorSpriteTilemap.Rotate270();
            floorObjectsTilemap.Rotate270();
            wallSpriteTilemap.Rotate270();
            wallObjectsTilemap.Rotate270();
        }
        
        [NaughtyAttributes.Button]
        public void MirrorHorz()
        {
            floorSpriteTilemap.MirrorHorizontally();
            floorObjectsTilemap.MirrorHorizontally();
            wallSpriteTilemap.MirrorHorizontally();
            wallObjectsTilemap.MirrorHorizontally();
        }
        
        [NaughtyAttributes.Button]
        public void MirrorVert()
        {
            floorSpriteTilemap.MirrorVertically();
            floorObjectsTilemap.MirrorVertically();
            wallSpriteTilemap.MirrorVertically();
            wallObjectsTilemap.MirrorVertically();
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
