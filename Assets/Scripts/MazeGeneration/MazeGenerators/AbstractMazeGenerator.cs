using UnityEngine;

namespace MazeGeneration.MazeGenerators
{
    public abstract class AbstractMazeGenerator : ScriptableObject
    {
        /// <summary>
        /// Generate maze.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public abstract bool[][] GenerateMaze(int width, int height);
    }

    public static class MazeGeneratorUtils
    {
        public static bool[][] ConstructMatrix(int width, int height, bool initVal=false)
        {
            bool[][] mat = new bool[height][];

            for (int i = 0; i < height; i++)
            {
                mat[i] = new bool[width];
                for (int j = 0; j < width; j++)
                {
                    mat[i][j] = initVal;
                }
            }
        
            return mat;
        }
    
        public static bool[][] ConstructMatrix(this AbstractMazeGenerator amg, int width, int height, bool initVal=false)
        {
            return ConstructMatrix(width, height, initVal);
        }
    
        public static bool[][] RandomMatrix(this AbstractMazeGenerator amg, int width, int height)
        {
            bool[][] mat = new bool[height][];

            for (int i = 0; i < height; i++)
            {
                mat[i] = new bool[width];
                for (int j = 0; j < width; j++)
                {
                    mat[i][j] = Random.value > 0.5f;
                }
            }
        
            return mat;
        }
    }
}