using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "BottomWallDetector", menuName = "Maze/InterestPointDetectors/BottomWallDetector", order = 0)]
    public class BottomWallDetector : InterestPointDetector
    {
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var wallPositions = maze.GetWallPositions();
            var walsBounds = maze.WallSpriteTilemap.RealCellBounds();
            var tilePos = new Vector3Int();
            var points = new List<Vector3Int>();

            foreach (var tilePosition in wallPositions)
            {
                tilePos.x = tilePosition.x;
                tilePos.y = tilePosition.y - 1;

                bool isMazeBottom = tilePosition.y == walsBounds.yMin;
                bool isBottom = !maze.WallSpriteTilemap.HasTile(tilePos);

                if (isBottom && !isMazeBottom)
                {
                    points.Add(tilePosition);
                }
            }

            return points;
        }
    }
}