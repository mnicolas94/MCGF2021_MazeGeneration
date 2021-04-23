using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    public abstract class InterestPointDetector : ScriptableObject
    {
        public abstract List<Vector3Int> GetInterestingPoints(Maze maze);
    }
}