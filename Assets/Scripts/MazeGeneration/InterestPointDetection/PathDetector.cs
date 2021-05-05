using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "PathDetector", menuName = "Maze/InterestPointDetectors/PathDetector", order = 0)]
    public class PathDetector : InterestPointDetector
    {
        [SerializeField] private Vector3Int from;
        [SerializeField] private Vector3Int to;

        [SerializeField] private bool pickRandomPoints;
        
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            Vector3Int f;
            Vector3Int t;
            if (pickRandomPoints)
            {
                f = maze.FloorSpriteTilemap.GetRandomTilePosition();
                t = maze.FloorSpriteTilemap.GetRandomTilePosition();
            }
            else
            {
                f = from;
                t = to;
            }
            
            var path = new List<Vector3Int>();
            MazePaths.GetDfsPath(maze.FloorSpriteTilemap, path, f, t);
            return path;
        }
    }
}