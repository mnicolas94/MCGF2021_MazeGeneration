using System;
using System.Collections.Generic;
using System.Numerics;
using MazeGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Character
{
    public class RandomMazeWalkerController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement characterMovement;

        private Maze _maze;
        private List<Vector3Int> _path;
        private Vector3 _moveToPosition;

        public List<Vector3Int> Path => _path;

        private void Start()
        {
            _maze = GameManager.Instance.Maze;
            _path = new List<Vector3Int>();
            ComputeNextPath();
        }

        private void FixedUpdate()
        {
            var targetPosition = NextMovePosition();

            Vector2 dir = targetPosition - transform.position;
            characterMovement.Move(dir);
        }

        private Vector3 NextMovePosition()
        {
            if (ArrivedAtPos(_moveToPosition))
            {
                _path.RemoveAt(0);
                
                while (_path.Count == 0)
                {
                    ComputeNextPath();
                }

                var nextTile = _path[0];
                var worldPos = _maze.Grid.CellToWorld(nextTile);
                _moveToPosition = worldPos;
            }
            
            return _moveToPosition;
        }

        private bool ArrivedAtPos(Vector3 targetPosition)
        {
            var position = transform.position;
            return (targetPosition - position).magnitude < 1E-1;
        }

        private void ComputeNextPath()
        {
            var currentPosition = GetCurrentTilePosition();
            var target = GetTargetPosition();
            MazePaths.GetDfsPath(_maze.FloorSpriteTilemap, _path, currentPosition, target);
            _moveToPosition = transform.position;
            NextMovePosition();
        }

        private Vector3Int GetCurrentTilePosition()
        {
            var position = transform.position;
            var tilePosition = _maze.Grid.WorldToCell(position);

            if (!_maze.FloorSpriteTilemap.HasTile(tilePosition))  // sometimes the wrong tile position is provided
            {
                tilePosition = GetNearestAdjacentTile(_maze.FloorSpriteTilemap, position, tilePosition);
            }
            
            return tilePosition;
        }
        
        private Vector3Int GetTargetPosition()
        {
            var currentPosition = GetCurrentTilePosition();

            var position = _maze.FloorSpriteTilemap.GetRandomTilePosition();
            while (position == currentPosition)
            {
                position = _maze.FloorSpriteTilemap.GetRandomTilePosition();
            }
            
            return position;
        }

        private Vector3Int GetNearestAdjacentTile(Tilemap tilemap, Vector3 position, Vector3Int tilePosition)
        {
            Vector3Int pos = Vector3Int.zero;

            var nearestTile = tilePosition;
            float minDistance = Mathf.Infinity;
            
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        pos.x = tilePosition.x + i;
                        pos.y = tilePosition.y + j;

                        if (tilemap.HasTile(pos))
                        {
                            var worldTilePosition = tilemap.CellToWorld(pos);
                            float dist = Vector3.Distance(position, worldTilePosition);
                            if (dist < minDistance)
                            {
                                minDistance = dist;
                                nearestTile = pos;
                            }
                        }
                    }
                }
            }

            return nearestTile;
        }
    }
}