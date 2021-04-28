using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "LeftMostWall", menuName = "Maze/InterestPointDetectors/LeftMostWall", order = 0)]
    public class LeftMostWall : InterestPointDetector
    {
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var wallPositions = maze.GetWallPositions();
            var walsBounds = maze.WallSpriteTilemap.RealCellBounds();
            var tilePos = new Vector3Int();
            var points = new List<Vector3Int>();
                                
            foreach (var tilePosition in wallPositions)
            {
                tilePos.x = tilePosition.x - 1;
                tilePos.y = tilePosition.y;
                                
                bool isMazeLeftWall = tilePosition.x == walsBounds.xMin;
                bool isLeftMost = !maze.WallSpriteTilemap.HasTile(tilePos);
                                
                if (isLeftMost && !isMazeLeftWall)
                {
                    points.Add(tilePosition);
                }
            }
                                
            return points;
        }
    }
}