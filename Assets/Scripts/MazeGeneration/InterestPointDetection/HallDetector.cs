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
            
            foreach (var tilePosition in floorPositions)
            {
                int neighbourValue = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i != 0 || j != 0)
                        {
                            tilePos.x = tilePosition.x + i;
                            tilePos.y = tilePosition.y + j;
                            if (maze.FloorSpriteTilemap.HasTile(tilePos))
                            {
                                neighbourValue += Math.Abs(i);
                                neighbourValue += Math.Abs(j);
                            }
                        }
                    }
                }
                if (neighbourValue > 0 && neighbourValue <= 2)
                {
                    points.Add(tilePosition);
                }
            }

            return points;
        }
    }
}