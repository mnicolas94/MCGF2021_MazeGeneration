using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Puzzles/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int mazeWidth;
        [SerializeField] private int mazeHeight;
        [SerializeField] private List<PuzzleData> puzzles;
        [SerializeField] private List<Vector2Int> quadrants;

        public int MazeWidth => mazeWidth;

        public int MazeHeight => mazeHeight;

        public List<PuzzleData> Puzzles => puzzles;

        public List<Vector2Int> Quadrants => quadrants;
    }
}