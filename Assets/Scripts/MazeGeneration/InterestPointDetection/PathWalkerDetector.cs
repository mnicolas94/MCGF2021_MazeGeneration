using System.Collections.Generic;
using Character;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "PathWalkerDetector", menuName = "Maze/InterestPointDetectors/PathWalkerDetector", order = 0)]
    public class PathWalkerDetector : InterestPointDetector
    {
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var walker = FindObjectOfType<RandomMazeWalkerController>();
            if (walker != null)
            {
                var path = walker.Path;
                return path;
            }

            return new List<Vector3Int>();
        }
    }
}