using System.Collections;
using System.Collections.Generic;
using MazeGeneration.MazeDecorators;
using MazeGeneration.MazeGenerators;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public class MazeController : MonoBehaviour
    {
        public static MazeController Instance;
    
        [SerializeField] private List<Maze> mazes;
        [SerializeField] private AbstractMazeGenerator generator;
        [SerializeField] private List<AbstractMazeDecorator> decorators;
    
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Maze GetMazeParent(GameObject go)
        {
            return go.GetComponentInParent<Maze>();
        }
    
        public void GenerateMaze(Maze maze, int width, int height, int runes)
        {
            maze.ClearMaze();
            var generatedMaze = generator.GenerateMaze(width - 2, height - 2);

            maze.PopulateMazeStructure(generatedMaze, width, height);
        
            // decorate
            foreach (var decorator in decorators)
            {
                decorator.DecorateMaze(maze);
            }
        
            StartCoroutine(NotifyMazeGeneration(maze));
        }

        private IEnumerator NotifyMazeGeneration(Maze maze)
        {
            maze.eventMazeChanged?.Invoke();
            yield return null;  // wait one frame for tile objects to get spawned
            maze.eventMazePostChanged?.Invoke();
        }
        
        [SerializeField] private int genWidth = 10;
        [SerializeField] private int genHeight = 10;
        [NaughtyAttributes.Button]
        void GenerateMaze()
        {
            foreach (var maze in mazes)
            {
                GenerateMaze(maze, genWidth, genHeight, 3);
            }
        }
    }
}
