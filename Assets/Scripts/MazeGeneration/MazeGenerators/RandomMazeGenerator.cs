using UnityEngine;

namespace MazeGeneration.MazeGenerators
{
    [CreateAssetMenu(fileName = "RandomMazeGenerator", menuName = "Maze/Generator/RandomMazeGenerator")]
    public class RandomMazeGenerator : AbstractMazeGenerator
    {
        public override bool[][] GenerateMaze(int width, int height)
        {
            return this.RandomMatrix(width, height);
        }
    }
}
