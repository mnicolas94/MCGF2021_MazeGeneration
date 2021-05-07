using System.Collections.Generic;
using Character;
using Character.Controllers;
using UnityEngine;

namespace MazeGeneration.MazeDecorators
{
    [CreateAssetMenu(fileName = "CharacterPositionerDecorator", menuName = "Maze/Decorators/CharacterPositionerDecorator", order = 0)]
    public class CharacterPositionerDecorator : AbstractMazeDecorator
    {
        [SerializeField] private Vector3 offset;
        
        public override void DecorateMaze(Maze maze, List<Vector3Int> mask)
        {
            var player = FindObjectOfType<PlayerController>().transform;
            float z = player.position.z;
            var position = NearestFloor(maze, Vector3.zero, mask);
            var tilePosition = maze.Grid.WorldToCell(position);
            
            position += offset;
            position.z = z;
            player.position = position;

            if (!mask.Contains(tilePosition))
            {
                mask.Add(tilePosition);
            }
        }
        
        private Vector3 NearestFloor(Maze maze, Vector3 position, List<Vector3Int> mask)
        {
            var floorPositions = maze.GetFloorPositions();

            float minSqrDist = Mathf.Infinity;
            Vector3 nearestPosition = Vector3.zero;
            foreach (var pos in floorPositions)
            {
                var worldPosition = maze.Grid.CellToWorld(pos);

                var dif = worldPosition - position;
                float sqrDist = dif.x * dif.x + dif.y * dif.y;
                bool notInMask = !mask.Contains(pos);
                if (sqrDist < minSqrDist && notInMask)
                {
                    minSqrDist = sqrDist;
                    nearestPosition = worldPosition;
                }
            }

            return nearestPosition;
        }
    }
}