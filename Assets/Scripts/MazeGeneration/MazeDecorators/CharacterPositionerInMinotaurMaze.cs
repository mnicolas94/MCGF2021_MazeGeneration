using System.Collections.Generic;
using Character.Controllers;
using UnityEngine;

namespace MazeGeneration.MazeDecorators
{
    [CreateAssetMenu(fileName = "CharacterPositionerInMinotaurMaze", menuName = "Maze/Decorators/CharacterPositionerInMinotaurMaze", order = 0)]
    public class CharacterPositionerInMinotaurMaze : AbstractMazeDecorator
    {
        [SerializeField] private Vector3 positionToPlace;
                
        public override void DecorateMaze(Maze maze, List<Vector3Int> mask)
        {
            var player = FindObjectOfType<PlayerController>().transform;
            float z = player.position.z;
            var position = NearestFloor(maze, positionToPlace);
            
            position.z = z;
            player.position = position;
        }
                
        private Vector3 NearestFloor(Maze maze, Vector3 position)
        {
            var floorPositions = maze.GetFloorPositions();

            float minSqrDist = Mathf.Infinity;
            Vector3 nearestPosition = Vector3.zero;
            foreach (var pos in floorPositions)
            {
                var worldPosition = maze.Grid.CellToWorld(pos);

                var dif = worldPosition - position;
                float sqrDist = dif.x * dif.x + dif.y * dif.y;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    nearestPosition = worldPosition;
                }
            }

            return nearestPosition;
        }
    }
}