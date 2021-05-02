using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "AllPositionsDetector", menuName = "Maze/InterestPointDetectors/AllPositionsDetector", order = 0)]
    public class AllPositionsDetector : InterestPointDetector
    {
        [SerializeField] private MazeTilemap mazeTilemap;

        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            switch (mazeTilemap)
            {
                case MazeTilemap.Floor:
                    return new List<Vector3Int>(maze.GetFloorPositions());
                case MazeTilemap.Walls:
                    return new List<Vector3Int>(maze.GetWallPositions());
                case MazeTilemap.FloorObjects:
                    return new List<Vector3Int>(maze.GetFloorObjectsPositions());
                case MazeTilemap.WallsObjects:
                    return new List<Vector3Int>(maze.GetWallObjectsPositions());
                default:
                    return new List<Vector3Int>(maze.GetFloorPositions());
            }
        }
    }
}