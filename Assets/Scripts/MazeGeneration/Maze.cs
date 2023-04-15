using System;
using System.Collections.Generic;
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

        public Grid Grid => grid;

        public Tilemap FloorSpriteTilemap => floorSpriteTilemap;

        public Tilemap FloorObjectsTilemap => floorObjectsTilemap;

        public Tilemap WallSpriteTilemap => wallSpriteTilemap;

        public Tilemap WallObjectsTilemap => wallObjectsTilemap;

        public TileBase FloorSpriteTile
        {
            get => floorSpriteTile;
            set => floorSpriteTile = value;
        }

        public TileBase WallSpriteTile
        {
            get => wallSpriteTile;
            set => wallSpriteTile = value;
        }

        public Tilemap GetTileMap(MazeTilemap tilemap)
        {
            switch (tilemap)
            {
                case MazeTilemap.Floor:
                    return floorSpriteTilemap;
                case MazeTilemap.Walls:
                    return wallSpriteTilemap;
                case MazeTilemap.FloorObjects:
                    return floorObjectsTilemap;
                case MazeTilemap.WallsObjects:
                    return wallObjectsTilemap;
                default:
                    return floorSpriteTilemap;
            }
        }
        
        public BoundsInt GetBounds()
        {
            var floorSpriteBounds = floorSpriteTilemap.RealCellBounds();
            var floorObjectsBounds = floorObjectsTilemap.RealCellBounds();
            var wallSpriteBounds = wallSpriteTilemap.RealCellBounds();
            var wallObjectsBounds = wallObjectsTilemap.RealCellBounds();

            int floorSpriteTilesCount = floorSpriteTilemap.GetTilesCount();
            int floorObjectsTilesCount = floorObjectsTilemap.GetTilesCount();
            int wallSpriteTilesCount = wallSpriteTilemap.GetTilesCount();
            int wallObjectsTilesCount = wallObjectsTilemap.GetTilesCount();
            
            int xMin = Int32.MaxValue;
            xMin = floorSpriteTilesCount  > 0 ? Math.Min(xMin, floorSpriteBounds.xMin) : xMin;
            xMin = floorObjectsTilesCount > 0 ? Math.Min(xMin, floorObjectsBounds.xMin) : xMin;
            xMin = wallSpriteTilesCount   > 0 ? Math.Min(xMin, wallSpriteBounds.xMin) : xMin;
            xMin = wallObjectsTilesCount  > 0 ? Math.Min(xMin, wallObjectsBounds.xMin) : xMin;
            
            int xMax = Int32.MinValue;
            xMax = floorSpriteTilesCount  > 0 ? Math.Max(xMax, floorSpriteBounds.xMax) : xMax;
            xMax = floorObjectsTilesCount > 0 ? Math.Max(xMax, floorObjectsBounds.xMax) : xMax;
            xMax = wallSpriteTilesCount   > 0 ? Math.Max(xMax, wallSpriteBounds.xMax) : xMax;
            xMax = wallObjectsTilesCount  > 0 ? Math.Max(xMax, wallObjectsBounds.xMax) : xMax;
            
            int yMin = Int32.MaxValue;
            yMin = floorSpriteTilesCount  > 0 ? Math.Min(yMin, floorSpriteBounds.yMin) : yMin;
            yMin = floorObjectsTilesCount > 0 ? Math.Min(yMin, floorObjectsBounds.yMin) : yMin;
            yMin = wallSpriteTilesCount   > 0 ? Math.Min(yMin, wallSpriteBounds.yMin) : yMin;
            yMin = wallObjectsTilesCount  > 0 ? Math.Min(yMin, wallObjectsBounds.yMin) : yMin;
            
            int yMax = Int32.MinValue;
            yMax = floorSpriteTilesCount  > 0 ? Math.Max(yMax, floorSpriteBounds.yMax) : yMax;
            yMax = floorObjectsTilesCount > 0 ? Math.Max(yMax, floorObjectsBounds.yMax) : yMax;
            yMax = wallSpriteTilesCount   > 0 ? Math.Max(yMax, wallSpriteBounds.yMax) : yMax;
            yMax = wallObjectsTilesCount  > 0 ? Math.Max(yMax, wallObjectsBounds.yMax) : yMax;
            
            BoundsInt bounds = new BoundsInt(xMin, yMin, 0, xMax - xMin, yMax - yMin, 1);
            return bounds;
        }

        public IEnumerable<Vector3Int> GetFloorPositions()
        {
            return floorSpriteTilemap.GetTilePositions();
        }
        
        public IEnumerable<Vector3Int> GetWallPositions()
        {
            return wallSpriteTilemap.GetTilePositions();
        }
        
        public IEnumerable<Vector3Int> GetFloorObjectsPositions()
        {
            return floorObjectsTilemap.GetTilePositions();
        }
        
        public IEnumerable<Vector3Int> GetWallObjectsPositions()
        {
            return wallObjectsTilemap.GetTilePositions();
        }
        
        public void PlaceDefaultFloorAtPosition(Vector3Int tilePosition)
        {
            RemoveTilesAtPos(tilePosition);
            floorSpriteTilemap.SetTile(tilePosition, floorSpriteTile);
        }
        
        public void PlaceDefaultWallAtPosition(Vector3Int tilePosition)
        {
            RemoveTilesAtPos(tilePosition);
            wallSpriteTilemap.SetTile(tilePosition, wallSpriteTile);
        }
        
        public void PlaceFloorAtPosition(Vector3Int tilePosition, TileBase floorTile)
        {
            RemoveTilesAtPos(tilePosition);
            floorSpriteTilemap.SetTile(tilePosition, floorTile);
        }
        
        public void PlaceWallAtPosition(Vector3Int tilePosition, TileBase wallTile)
        {
            RemoveTilesAtPos(tilePosition);
            wallSpriteTilemap.SetTile(tilePosition, wallTile);
        }
        
        public void PlaceFloorObjectAtPosition(Vector3Int tilePosition, TileBase floorObject)
        {
            floorObjectsTilemap.SetTile(tilePosition, floorObject);
        }
        
        public void PlaceWallObjectAtPosition(Vector3Int tilePosition, TileBase wallObject)
        {
            wallObjectsTilemap.SetTile(tilePosition, wallObject);
        }
        
        public void RemoveTilesAtPos(Vector3Int tilePosition)
        {
            floorSpriteTilemap.SetTile(tilePosition, null);
            floorObjectsTilemap.SetTile(tilePosition, null);
            wallSpriteTilemap.SetTile(tilePosition, null);
            wallObjectsTilemap.SetTile(tilePosition, null);
        }

        public void MoveTile(Tilemap tm, Vector3Int from, Vector3Int to)
        {
            if (tm.HasTile(from) && !tm.HasTile(to))
            {
                var tile = tm.GetTile(from);
//                var instantiatedGameObject = tm.GetInstantiatedObject(from);
                tm.SetTile(to, tile);
                tm.SetTile(from, null);
            }
        }

        public void MoveTiles(Vector3Int from, Vector3Int to)
        {
            MoveTile(floorSpriteTilemap, from, to);
            MoveTile(floorObjectsTilemap, from, to);
            MoveTile(wallSpriteTilemap, from, to);
            MoveTile(wallObjectsTilemap, from, to);
        }

        public void RepeatHorizontally(int dir)
        {
            dir = Mathf.Clamp(dir, -1, 1);
            var bounds = GetBounds();
            int columnSource = dir == 1 ? bounds.xMin : bounds.xMax - 1;
            int columnDestiny = dir == 1 ? bounds.xMax : bounds.xMin - 1;

            var from = new Vector3Int();
            var to = new Vector3Int();
            for (int i = bounds.yMin; i < bounds.yMax; i++)
            {
                from.x = columnSource;
                from.y = i;
                to.x = columnDestiny;
                to.y = i;
                MoveTiles(from, to);
            }
        }
        
        public void RepeatVertically(int dir)
        {
            dir = Mathf.Clamp(dir, -1, 1);
            var bounds = GetBounds();
            int rowSource = dir == 1 ? bounds.yMin : bounds.yMax - 1;
            int rowDestiny = dir == 1 ? bounds.yMax : bounds.yMin - 1;

            var from = new Vector3Int();
            var to = new Vector3Int();
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                from.x = i;
                from.y = rowSource;
                to.x = i;
                to.y = rowDestiny;
                MoveTiles(from, to);
            }
        }

        public void RepeatHorizontally(int dir, int times)
        {
            for (int i = 0; i < times; i++)
            {
                RepeatHorizontally(dir);
            }
        }
        
        public void RepeatVertically(int dir, int times)
        {
            for (int i = 0; i < times; i++)
            {
                RepeatVertically(dir);
            }
        }

        [NaughtyAttributes.Button]
        public void RepeatUp()
        {
            RepeatVertically(1);
        }
        
        [NaughtyAttributes.Button]
        public void RepeatDown()
        {
            RepeatVertically(-1);
        }
        
        [NaughtyAttributes.Button]
        public void RepeatRight()
        {
            RepeatHorizontally(1);
        }
        
        [NaughtyAttributes.Button]
        public void RepeatLeft()
        {
            RepeatHorizontally(-1);
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
        public void ClearMaze()
        {
            floorSpriteTilemap.ClearAllTiles();
            floorObjectsTilemap.ClearAllTiles();
            wallSpriteTilemap.ClearAllTiles();
            wallObjectsTilemap.ClearAllTiles();
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
            var offset = wallSpriteTilemap.Center();
            floorSpriteTilemap.MoveByOffset(offset);
            floorObjectsTilemap.MoveByOffset(offset);
            wallObjectsTilemap.MoveByOffset(offset);
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
        public void MirrorHorizontally()
        {
            floorSpriteTilemap.MirrorHorizontally();
            floorObjectsTilemap.MirrorHorizontally();
            wallSpriteTilemap.MirrorHorizontally();
            wallObjectsTilemap.MirrorHorizontally();
        }
        
        [NaughtyAttributes.Button]
        public void MirrorVertically()
        {
            floorSpriteTilemap.MirrorVertically();
            floorObjectsTilemap.MirrorVertically();
            wallSpriteTilemap.MirrorVertically();
            wallObjectsTilemap.MirrorVertically();
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

                    if (i == 0 || j == 0 || i == height - 1 || j == width - 1) // edge walls
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
    
    public enum MazeTilemap
    {
        Floor,
        Walls,
        FloorObjects,
        WallsObjects,
    }
}
