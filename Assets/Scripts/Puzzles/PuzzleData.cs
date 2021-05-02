using System.Collections.Generic;
using MazeGeneration;
using MazeGeneration.MazeDecorators;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "PuzzleData", menuName = "Puzzles/PuzzleData", order = 0)]
    public class PuzzleData : ScriptableObject
    {
        [SerializeField] private List<MazeData> roomsData;
        [SerializeField] private List<AbstractMazeDecorator> decorators;

        public List<MazeData> RoomsData => roomsData;

        public List<AbstractMazeDecorator> Decorators => decorators;
    }
}