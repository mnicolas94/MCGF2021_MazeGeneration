using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "HallDetector", menuName = "Maze/InterestPointDetectors/HallDetector", order = 0)]
    public class HallDetector : InterestPointDetector
    {
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var floorPositions = maze.GetFloorPositions();
            var tilePos = new Vector3Int();
            var points = new List<Vector3Int>();

            int[] tilesValues = new int[9];
            foreach (var tilePosition in floorPositions)
            {
                int neighbourValue = 0;
                int tileIndex = 0;
                for (int i = -1; i <= 1; i++)
                {
                    int absI = Math.Abs(i);
                    
                    for (int j = -1; j <= 1; j++)
                    {
                        int absJ = Math.Abs(j);
                        
                        tilePos.x = tilePosition.x + i;
                        tilePos.y = tilePosition.y + j;
                        int hasTile = maze.FloorSpriteTilemap.HasTile(tilePos) ? 1 : 0;
                        int xor = absI ^ absJ;  // 1 if is left, right, top or bottom neighbour, 0 otherwise
                        neighbourValue += xor * hasTile;

                        tilesValues[tileIndex] = hasTile;

                        tileIndex++;
                    }
                }

                bool cornerSquare = tilesValues[0] + tilesValues[1] + tilesValues[3] == 3;
                cornerSquare |= tilesValues[2] + tilesValues[1] + tilesValues[5] == 3;
                cornerSquare |= tilesValues[6] + tilesValues[3] + tilesValues[7] == 3;
                cornerSquare |= tilesValues[8] + tilesValues[5] + tilesValues[7] == 3;

                bool isHall = neighbourValue > 0 && neighbourValue <= 2;
                if (isHall && !cornerSquare)
                {
                    points.Add(tilePosition);
                }
            }

            return points;
        }
    }
}