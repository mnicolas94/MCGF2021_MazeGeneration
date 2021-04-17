using UnityEngine;

namespace MazeGeneration.MazeGenerators
{
    [CreateAssetMenu(fileName = "BinaryMazeGenerator", menuName = "Maze/Generator/BinaryMazeGenerator")]
    public class BinaryMazeGenerator : AbstractMazeGenerator
    {
        public override bool[][] GenerateMaze(int width, int height)
        {
            bool[][] mat = this.ConstructMatrix(width, height);
            for (int x = 0; x < width; x += 2)
            {
                for (int y = 0; y < height; y += 2)
                {
                    mat[y][x] = true;
                    if (x != 0 || y != 0)
                    {
                        bool horizontal;
                        if (x == 0)
                            horizontal = false;
                        else if (y == 0)
                            horizontal = true;
                        else
                            horizontal = Random.value > 0.5f;

                        if (horizontal)
                            mat[y][x - 1] = true;
                        else
                            mat[y - 1][x] = true;

                        if (x + 2 == width && horizontal)
                            mat[y][x + 1] = true;
                        if (y + 2 == height && !horizontal)
                            mat[y + 1][x] = true;
                    }
                }
            }

            return mat;
        }
    }
}
