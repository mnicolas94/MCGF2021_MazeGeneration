using System.Collections;
using System.Collections.Generic;
using MazeGeneration.MazeDecorators;
using MazeGeneration.MazeGenerators;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace MazeGeneration
{
    public class MazeController : MonoBehaviour
    {
        public static MazeController Instance;
    
        [SerializeField] private List<Maze> mazes;
        [SerializeField] private AbstractMazeGenerator generator;
        [SerializeField] private List<AbstractMazeDecorator> decorators;

        [SerializeField] private List<Maze> rooms;

        private MazeChunk[][] _mazeChunks;
    
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
    
        public void GenerateMaze(Maze maze, int width, int height)
        {
            maze.ClearMaze();
            var generatedMaze = generator.GenerateMaze(width - 2, height - 2);
            AddRoom(generatedMaze, width, height);
            maze.PopulateWallsAndFloor(generatedMaze, width, height);
        
            // decorate
            foreach (var decorator in decorators)
            {
                decorator.DecorateMaze(maze);
            }
        
            StartCoroutine(NotifyMazeGeneration(maze));
        }

        private void AddRoom(bool[][] maze, int mazeWidth, int mazeHeight)
        {
            int minRoomWidth = 5;
            int minRoomHeight = 5;
            int maxRoomWidth = mazeWidth - 6;
            int maxRoomHeight = mazeHeight - 6;

            if (maxRoomWidth > minRoomWidth && maxRoomHeight > minRoomHeight)
            {
                int roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
                int roomHeight = Random.Range(minRoomHeight, maxRoomHeight);
                
                roomWidth += 1 - roomWidth % 2;  // make sure is an odd number
                roomHeight += 1 - roomHeight % 2;
                
                int minXPos = 2;
                int minYPos = 2;
                int maxXPos = mazeWidth - roomWidth - 3;
                int maxYPos = mazeHeight - roomHeight - 3;
                int leftPos = Random.Range(minXPos, maxXPos);
                int bottomPos = Random.Range(minYPos, maxYPos);

                leftPos += 1 - leftPos % 2;  // make sure is an odd number
                bottomPos += 1 - bottomPos % 2;

                int rightPos = leftPos + roomWidth - 1;
                int topPos = bottomPos + roomHeight - 1;

                for (int i = leftPos; i <= rightPos; i++)  // construct room
                {
                    for (int j = bottomPos; j <= topPos; j++)
                    {
                        if (i == leftPos || i == rightPos || j == bottomPos || j == topPos)  // room walls
                        {
                            maze[j][i] = false;
                        }
                        else  // room floor
                        {
                            maze[j][i] = true;
                        }
                    }
                }

                // make sure room does not prevents maze to be fully traversable
                for (int i = leftPos; i <= rightPos; i++)
                {
                    maze[bottomPos - 1][i] = true;
                    maze[topPos + 1][i] = true;
                }
                
                for (int i = bottomPos; i <= topPos; i++)
                {
                    maze[i][leftPos - 1] = true;
                    maze[i][rightPos + 1] = true;
                }
                
                // open entrances
                int numberEntrances = Random.Range(1, 5);  // [1,4]
                var entrancesCoordinate = new List<Vector2Int>
                {
                    new Vector2Int(Random.Range(leftPos + 1, rightPos), bottomPos),
                    new Vector2Int(Random.Range(leftPos + 1, rightPos), topPos),
                    new Vector2Int(leftPos, Random.Range(bottomPos + 1, topPos)),
                    new Vector2Int(rightPos, Random.Range(bottomPos + 1, topPos)),
                };

                while (numberEntrances > 0)
                {
                    int randIndex = Random.Range(0, entrancesCoordinate.Count);

                    var entrance = entrancesCoordinate[randIndex];
                    entrancesCoordinate.RemoveAt(randIndex);

                    maze[entrance.y][entrance.x] = true;
                    
                    numberEntrances--;
                }
            }
        }

        private void AddRoom(Maze room)
        {
            
        }

        private Vector2Int GetRandomPosForRoom(Maze room)
        {
            var bounds = room.WallSpriteTilemap.RealCellBounds();

            return default;
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
                GenerateMaze(maze, genWidth, genHeight);
            }
        }
    }
}
