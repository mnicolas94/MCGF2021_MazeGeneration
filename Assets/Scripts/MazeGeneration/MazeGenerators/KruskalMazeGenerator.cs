using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.MazeGenerators
{
    [CreateAssetMenu(fileName = "KruskalMazeGenerator", menuName = "Maze/Generator/KruskalMazeGenerator")]
    public class KruskalMazeGenerator : AbstractMazeGenerator
    {
        public override bool[][] GenerateMaze(int width, int height)
        {
            bool[][] mat = this.ConstructMatrix(width, height);
            int[][] sets = InitializeSets(width, height);

            List<Element> elements = new List<Element>();
            int i = 1;
            for (int y = 0; y < height; y += 2)
            {
                for (int x = 0; x < width; x += 2)
                {
                    if ((y + 2) < height)
                    {
                        elements.Add(new Element(x, y, false));
                    }
                    if ((x + 2) < width)
                    {
                        elements.Add(new Element(x, y, true));
                    }

                    sets[y][x] = i;
                    i++;
                }
            }

            while (elements.Count > 0)
            {
                int randIndex = Random.Range(0, elements.Count);
                var e = elements[randIndex];
                elements.RemoveAt(randIndex);
                int x = e.x;
                int y = e.y;
                if (e.horizontal)
                {
                    if (sets[y][x + 2] != sets[y][x])
                    {
                        Replace(sets, sets[y][x + 2], sets[y][x]);
                        for (int j = 0; j < 3; j++)
                        {
                            mat[y][x + j] = true;
                        }
                    }
                }
                else
                {
                    if (sets[y + 2][x] != sets[y][x])
                    {
                        Replace(sets, sets[y + 2][x], sets[y][x]);
                        for (int j = 0; j < 3; j++)
                        {
                            mat[y + j][x] = true;
                        }
                    }
                }
            }
        
            return mat;
        }

        private int[][] InitializeSets(int width, int height)
        {
            int[][] sets = new int[height][];

            for (int i = 0; i < height; i++)
                sets[i] = new int[width];
        
            return sets;
        }
    
        private void Replace(int[][] sets, int toReplace, int replaceWith)
        {
            for (int y = 0; y < sets.Length; y += 2)
            {
                for (int x = 0; x < sets[y].Length; x += 2)
                {
                    if (sets[y][x] == toReplace)
                        sets[y][x] = replaceWith;
                }   
            }
        }
    }

    struct Element
    {
        public int y;
        public int x;
        public bool horizontal;

        public Element(int x, int y, bool horizontal)
        {
            this.x = x;
            this.y = y;
            this.horizontal = horizontal;
        }
    }
}