using System;
using MazeGeneration;
using UnityEngine;

namespace DefaultNamespace
{
    public class RepeatMazeWhenCharacterFarFromCenter : MonoBehaviour
    {
        [SerializeField] private Maze maze;
        [SerializeField] private Transform character;
        [SerializeField] private int distanceToRepeat;

        private void Update()
        {
            var position = character.position;
            var tilePosition = maze.Grid.WorldToCell(position);
            var mazeBounds = maze.GetBounds();
            var boundsCenter = mazeBounds.center;

            var horizontalDistance = tilePosition.x - boundsCenter.x;
            var verticalDistance = tilePosition.y - boundsCenter.y;

            if (horizontalDistance > distanceToRepeat)
            {
                int dir = Math.Sign(horizontalDistance);
                maze.RepeatHorizontally(dir, distanceToRepeat);
            }
            
            if (verticalDistance > distanceToRepeat)
            {
                int dir = Math.Sign(verticalDistance);
                maze.RepeatVertically(dir, distanceToRepeat);
            }
        }
    }
}