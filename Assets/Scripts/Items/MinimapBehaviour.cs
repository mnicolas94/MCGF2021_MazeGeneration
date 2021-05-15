using System;
using System.Collections.Generic;
using Character;
using MazeGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Items
{
    public class MinimapBehaviour : MonoBehaviour
    {
        [SerializeField] private Maze maze;
        [SerializeField] private Tilemap minimapTilemap;
        
        [SerializeField] private TileBase playerTileMinimap;
        [SerializeField] private TileBase floorTileMinimap;
        [SerializeField] private TileBase wallTileMinimap;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private LineOfSightData lineOfSightData;

        private void Update()
        {
            var bounds = GetLocalWorldBounds();
            var tilesPosition = GetTilesPositionsInsideBounds(bounds);
            tilesPosition = FilterTilesInsideRadius(tilesPosition);
//            tilesPosition = FilterNewTilesForMinimap(tilesPosition);

            foreach (var (worldPosition, tilePosition) in tilesPosition)
            {
                PutTileInMinimap(tilePosition);
            }

            SetPlayerTile();
        }

        private void SetPlayerTile()
        {
            var playerTilePosition = GetPlayerTilePosition();
//            if (!maze.FloorSpriteTilemap.HasTile(playerTilePosition))
//            {
//                var playerWorldPosition = GetPlayerWorldPosition();
//                playerTilePosition = maze.FloorSpriteTilemap.GetNearestAdjacentTile(playerWorldPosition, playerTilePosition);
//            }
            
            minimapTilemap.SetTile(playerTilePosition, playerTileMinimap);
        }

        public float verticalOffset;
        private Vector3 GetPlayerWorldPosition()
        {
            var playerPosition = playerTransform.position + Vector3.up * verticalOffset;
            return playerPosition;
        }
        
        private Vector3Int GetPlayerTilePosition()
        {
            var playerPosition = GetPlayerWorldPosition();
            var playerTilePosition = maze.Grid.WorldToCell(playerPosition);
            return playerTilePosition;
        }
        
        private BoundsInt GetLocalWorldBounds()
        {
            var playerTilePosition = GetPlayerTilePosition();
            int radius = Mathf.RoundToInt(lineOfSightData.MaxLineOfSightRadius);
            int xmin = playerTilePosition.x - radius + 1;
            int ymin = playerTilePosition.y - radius + 1;
            int size = radius * 2;
            size += size % 2;
            BoundsInt bounds = new BoundsInt(xmin, ymin, 0, size, size, 1);
            return bounds;
        }

        private IEnumerable<(Vector3, Vector3Int)> GetTilesPositionsInsideBounds(BoundsInt bounds)
        {
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                for (int j = bounds.yMin; j < bounds.yMax; j++)
                {
                    var tilePosition = new Vector3Int(i, j, 0);
                    var worldPosition = maze.Grid.CellToWorld(tilePosition);
                    yield return (worldPosition, tilePosition);
                }
            }
        }
        
        private IEnumerable<(Vector3, Vector3Int)> FilterTilesInsideRadius(IEnumerable<(Vector3, Vector3Int)> tilesPosition)
        {
            foreach (var (worldPosition, tilePosition) in tilesPosition)
            {
                if (lineOfSightData.IsInsideRadius(worldPosition))
                {
                    yield return (worldPosition, tilePosition);
                }
            }
        }
        
        private IEnumerable<(Vector3, Vector3Int)> FilterNewTilesForMinimap(IEnumerable<(Vector3, Vector3Int)> tilesPosition)
        {
            foreach (var (worldPosition, tilePosition) in tilesPosition)
            {
                bool alreadyExists = minimapTilemap.HasTile(tilePosition);
                bool isPlayerTile = minimapTilemap.GetTile(tilePosition) == playerTileMinimap;
                if (!alreadyExists || isPlayerTile)
                {
                    yield return (worldPosition, tilePosition);
                }
            }
        }

        private void PutTileInMinimap(Vector3Int tilePosition)
        {
            TileBase mazeTile = null;
            if (maze.WallSpriteTilemap.HasTile(tilePosition))
            {
                mazeTile = wallTileMinimap;
            }
            
            if (maze.FloorSpriteTilemap.HasTile(tilePosition))
            {
                mazeTile = floorTileMinimap;
            }

            minimapTilemap.SetTile(tilePosition, mazeTile);
        }
    }

    [Serializable]
    public struct TileToTile
    {
        public TileBase mazeTile;
        public TileBase minimapTile;
    }
}