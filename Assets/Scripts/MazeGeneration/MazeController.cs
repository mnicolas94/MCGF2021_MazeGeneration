﻿using System;
using System.Collections;
using System.Collections.Generic;
 using MazeGeneration.MazeDecorators;
using MazeGeneration.MazeGenerators;
using UnityEngine;
 using Utils;
using Random = UnityEngine.Random;

namespace MazeGeneration
{
    public class MazeController : MonoBehaviour
    {
        public static MazeController Instance;
    
        [SerializeField] private List<Maze> mazes;
        [SerializeField] private AbstractMazeGenerator generator;
        [SerializeField] private List<AbstractMazeDecorator> decorators;
        
        [SerializeField] private List<MazeData> rooms;
        
        private List<AbstractMazeDecorator> _alternativeDecorators;

        public List<AbstractMazeDecorator> AlternativeDecorators
        {
            get
            {
                if (_alternativeDecorators == null)
                {
                    _alternativeDecorators = new List<AbstractMazeDecorator>();
                }
                return _alternativeDecorators;
            }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        public void GenerateMaze(Maze maze, int width, int height, List<MazeData> roomsToAdd, List<Vector2Int> quadrants)
        {
            maze.ClearMaze();
            var generatedMaze = generator.GenerateMaze(width - 2, height - 2);
            maze.PopulateWallsAndFloor(generatedMaze, width, height);
            
            var roomsMask = AddRooms(maze, width, height, roomsToAdd, quadrants);
        
            // decorate
            foreach (var decorator in decorators)
            {
                decorator.DecorateMaze(maze, roomsMask);
            }
            
            foreach (var decorator in AlternativeDecorators)
            {
                decorator.DecorateMaze(maze, roomsMask);
            }
        
            StartCoroutine(NotifyMazeGeneration(maze));
        }

        private List<Vector3Int> AddRooms(Maze maze, int width, int height, List<MazeData> roomsToAdd, List<Vector2Int> quadrants)
        {
            List<Vector3Int> mask = new List<Vector3Int>();
            var mazeWallsBounds = maze.WallSpriteTilemap.RealCellBounds();
            int minXWallBounds = mazeWallsBounds.xMin;
            int minYWallBounds = mazeWallsBounds.yMin;
            var roomsCopy = new List<MazeData>(roomsToAdd);
            
            while (roomsCopy.Count > 0 && quadrants.Count > 0)
            {
                var roomIndex = Random.Range(0, roomsCopy.Count);
                var quadrantIndex = Random.Range(0, quadrants.Count);

                var roomData = roomsCopy[roomIndex];
                var roomPrefab = roomData.maze;
                var instantiatedRoom = Instantiate(roomPrefab);
                var quadrant = quadrants[quadrantIndex];
                roomsCopy.RemoveAt(roomIndex);
                quadrants.RemoveAt(quadrantIndex);
                
                instantiatedRoom.Center();
                if (roomData.canBeRotated)
                {
                    RotateAndMirrorRandomly(instantiatedRoom);
                }
                
                int quadrantWidth = width / 3;
                int quadrantHeight = height / 3;
                int minXQuadrant = quadrant.x * quadrantWidth + minXWallBounds;
                int minYQuadrant = quadrant.y * quadrantHeight + minYWallBounds;
                BoundsInt bounds = new BoundsInt(
                    minXQuadrant, minYQuadrant, 0,
                    quadrantWidth, quadrantHeight, 1);
                Vector3Int offset = GetRandomPosForRoom(instantiatedRoom, bounds);
                AddRoom(instantiatedRoom, offset, maze, mask);
#if UNITY_EDITOR
                DestroyImmediate(instantiatedRoom.gameObject);
#else
                Destroy(instantiatedRoom.gameObject);
#endif
            }

            return mask;
        }

        private void AddRoom(Maze room, Vector3Int mazeOffset, Maze maze, List<Vector3Int> mask)
        {
            var roomBounds = room.GetBounds();
            var roomOffset = roomBounds.size / 2;
            var offset = roomOffset + mazeOffset;
            
            foreach (var floorPositions in room.GetFloorPositions())
            {
                var offseted = floorPositions + offset;
                var tile = room.FloorSpriteTilemap.GetTile(floorPositions);
                maze.PlaceFloorAtPosition(offseted, tile);
                if (!mask.Contains(offseted))
                {
                    mask.Add(offseted);
                }
            }
            
            foreach (var wallPositions in room.GetWallPositions())
            {
                var offseted = wallPositions + offset;
                var tile = room.WallSpriteTilemap.GetTile(wallPositions);
                maze.PlaceWallAtPosition(offseted, tile);
                if (!mask.Contains(offseted))
                {
                    mask.Add(offseted);
                }
            }
            
            foreach (var floorPositions in room.GetFloorObjectsPositions())
            {
                var offseted = floorPositions + offset;
                var tile = room.FloorObjectsTilemap.GetTile(floorPositions);
                maze.PlaceFloorObjectAtPosition(offseted, tile);
                if (!mask.Contains(offseted))
                {
                    mask.Add(offseted);
                }
            }
            
            foreach (var wallPositions in room.GetWallObjectsPositions())
            {
                var offseted = wallPositions + offset;
                var tile = room.WallObjectsTilemap.GetTile(wallPositions);
                maze.PlaceWallObjectAtPosition(offseted, tile);
                if (!mask.Contains(offseted))
                {
                    mask.Add(offseted);
                }
            }
        }

        private Vector3Int GetRandomPosForRoom(Maze room, BoundsInt mazeBounds)
        {
            var roomBounds = room.GetBounds();

            int xMin = mazeBounds.xMin + 1;
            int xMax = xMin + mazeBounds.size.x - roomBounds.size.x - 1;
            
            int yMin = mazeBounds.yMin + 1;
            int yMax = yMin + mazeBounds.size.y - roomBounds.size.y - 1;

            int xPos = Random.Range(xMin, xMax);
            int yPos = Random.Range(yMin, yMax);

            return new Vector3Int(xPos, yPos, 0);
        }

        private void RotateAndMirrorRandomly(Maze room)
        {
            if (Random.value > 0.5f)
            {
                room.MirrorHorizontally();
            }
            
            if (Random.value > 0.5f)
            {
                room.MirrorVertically();
            }

            int rotation = Random.Range(0, 4);
            switch (rotation)
            {
                case 1:
                    room.Rotate90();
                    break;
                case 2:
                    room.Rotate180();
                    break;
                case 3:
                    room.Rotate270();
                    break;
                default:
                    break;
            }
        }
        
        private IEnumerator NotifyMazeGeneration(Maze maze)
        {
            maze.eventMazeChanged?.Invoke();
            yield return null;  // wait one frame for tile objects to get spawned
            maze.eventMazePostChanged?.Invoke();
        }
        
        [SerializeField] private int genWidth = 10;
        [SerializeField] private int genHeight = 10;
        [NaughtyAttributes.Button]
        public void GenerateMaze()
        {
            foreach (var maze in mazes)
            {
                GenerateMaze(maze, genWidth, genHeight, rooms, GetDefaultQuadrants());
            }
        }
        
        public void GenerateMaze(List<MazeData> roomsToAdd)
        {
            foreach (var maze in mazes)
            {
                GenerateMaze(maze, genWidth, genHeight, roomsToAdd, GetDefaultQuadrants());
            }
        }
        
        public void GenerateMaze(List<MazeData> roomsToAdd, List<Vector2Int> quadrants)
        {
            foreach (var maze in mazes)
            {
                GenerateMaze(maze, genWidth, genHeight, roomsToAdd, quadrants);
            }
        }

        private List<Vector2Int> GetDefaultQuadrants()
        {
            var quadrants = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 0),
                //new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(2, 0),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2),
            };
            return quadrants;
        }
    }

    [Serializable]
    public struct MazeData
    {
        public Maze maze;
        public bool canBeRotated;
    }
}
