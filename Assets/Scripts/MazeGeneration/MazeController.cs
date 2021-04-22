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
//            AddRoom(generatedMaze, width, height);
            maze.PopulateWallsAndFloor(generatedMaze, width, height);
            
            AddRooms(maze, width, height);
        
            // decorate
            foreach (var decorator in decorators)
            {
                decorator.DecorateMaze(maze);
            }
        
            StartCoroutine(NotifyMazeGeneration(maze));
        }

        private void AddRooms(Maze maze, int width, int height)
        {
            var mazeWallsBounds = maze.WallSpriteTilemap.RealCellBounds();
            int minXWallBounds = mazeWallsBounds.xMin;
            int minYWallBounds = mazeWallsBounds.yMin;
            var roomsCopy = new List<Maze>(rooms);
            var quadrants = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(2, 0),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2),
            };
            
            while (roomsCopy.Count > 0 && quadrants.Count > 0)
            {
                var roomIndex = Random.Range(0, roomsCopy.Count);
                var quadrantIndex = Random.Range(0, quadrants.Count);

                var room = roomsCopy[roomIndex];
                var quadrant = quadrants[quadrantIndex];
                roomsCopy.RemoveAt(roomIndex);
                quadrants.RemoveAt(quadrantIndex);
                
                room.Center();
                RotateAndMirrorRandomly(room);
                
                int quadrantWidth = width / 3;
                int quadrantHeight = height / 3;
                int minXQuadrant = quadrant.x * quadrantWidth + minXWallBounds;
                int minYQuadrant = quadrant.y * quadrantHeight + minYWallBounds;
                BoundsInt bounds = new BoundsInt(
                    minXQuadrant, minYQuadrant, 0,
                    quadrantWidth, quadrantHeight, 1);
                Vector3Int offset = GetRandomPosForRoom(room, bounds);
                AddRoom(room, offset, maze);
            }
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

        private void AddRoom(Maze room, Vector3Int mazeOffset, Maze maze)
        {
            var roomBounds = room.GetBounds();
            var roomOffset = roomBounds.size / 2;
            var offset = roomOffset + mazeOffset;
            
            foreach (var floorPositions in room.GetFloorPositions())
            {
                var offseted = floorPositions + offset;
                maze.PlaceFloorAtPosition(offseted);
            }
            
            foreach (var wallPositions in room.GetWallPositions())
            {
                var offseted = wallPositions + offset;
                maze.PlaceWallAtPosition(offseted);
            }
            
            foreach (var floorPositions in room.GetFloorObjectsPositions())
            {
                var offseted = floorPositions + offset;
                var tile = room.FloorObjectsTilemap.GetTile(floorPositions);
                maze.PlaceFloorObjectAtPosition(offseted, tile);
            }
            
            foreach (var wallPositions in room.GetWallObjectsPositions())
            {
                var offseted = wallPositions + offset;
                var tile = room.WallObjectsTilemap.GetTile(wallPositions);
                maze.PlaceWallObjectAtPosition(offseted, tile);
            }
        }

        private Vector3Int GetRandomPosForRoom(Maze room, BoundsInt mazeBounds)
        {
            var roomBounds = room.GetBounds();

            int xMin = mazeBounds.xMin + 1;
            int xMax = xMin + mazeBounds.size.x - roomBounds.size.x - 1;
            
            int yMin = mazeBounds.yMin + 1;
            int yMax = yMin + mazeBounds.size.y - roomBounds.size.y - 1;

            int xPos = Random.Range(xMin, xMax);
            int yPos = Random.Range(yMin, yMax);

            return new Vector3Int(xPos, yPos, 0);
        }

        private void RotateAndMirrorRandomly(Maze room)
        {
            if (Random.value > 0.5f)
            {
                room.MirrorHorz();
            }
            
            if (Random.value > 0.5f)
            {
                room.MirrorVert();
            }

            int rotation = Random.Range(0, 4);
            switch (rotation)
            {
                case 1:
                    room.Rotate90();
                    break;
                case 2:
                    room.Rotate180();
                    break;
                case 3:
                    room.Rotate270();
                    break;
                default:
                    break;
            }
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
        public void GenerateMaze()
        {
            foreach (var maze in mazes)
            {
                GenerateMaze(maze, genWidth, genHeight);
            }
        }
    }
}
