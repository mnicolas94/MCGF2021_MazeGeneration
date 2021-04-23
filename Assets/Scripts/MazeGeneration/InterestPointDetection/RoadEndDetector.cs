using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "RoadEndDetector", menuName = "Maze/InterestPointDetectors/RoadEndDetector", order = 0)]
    public class RoadEndDetector : InterestPointDetector
    {
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var floorPositions = maze.GetFloorPositions();
            var tilePos = new Vector3Int();
            var points = new List<Vector3Int>();

            foreach (var tilePosition in floorPositions)
            {
                int adjacentsCount = 0;
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
                        adjacentsCount += xor * hasTile;
                    }
                }

                bool isRoandEnd = adjacentsCount == 1;
                if (isRoandEnd)
                {
                    points.Add(tilePosition);
                }
            }

            return points;
        }
    }
}