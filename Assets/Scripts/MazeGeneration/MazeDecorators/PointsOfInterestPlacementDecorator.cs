using MazeGeneration.InterestPointDetection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration.MazeDecorators
{
    [CreateAssetMenu(fileName = "PointsOfInterestPlacementDecorator", menuName = "Maze/Decorators/PointsOfInterestPlacementDecorator", order = 0)]
    public class PointsOfInterestPlacementDecorator : AbstractMazeDecorator
    {
        [SerializeField] private InterestPointDetector detector;
        [SerializeField] private MazeTilemap tilemapToPlace;
        [SerializeField] private TileBase tile;
        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;
        
        public override void DecorateMaze(Maze maze)
        {
            var points = detector.GetInterestingPoints(maze);
            int count = Random.Range(minCount, maxCount + 1);

            var tilemap = maze.GetTileMap(tilemapToPlace);
            
            while (count >= 0 && points.Count > 0)
            {
                int pointIndex = Random.Range(0, points.Count);
                var point = points[pointIndex];
                points.RemoveAt(pointIndex);
                
                tilemap.SetTile(point, tile);

                count--;
            }
        }
    }
}