using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration.InterestPointDetection
{
    [CreateAssetMenu(fileName = "EquidistantPointsSelector", menuName = "Maze/InterestPointDetectors/EquidistantPointsSelector", order = 0)]
    public class EquidistantPointsSelector : InterestPointDetector
    {
        [SerializeField] private InterestPointDetector detector;
        [SerializeField] private int selectionCount;
        public override List<Vector3Int> GetInterestingPoints(Maze maze)
        {
            var selectedPoints = new List<Vector3Int>();
            var points = detector.GetInterestingPoints(maze);

            float sqrfloat = Mathf.Sqrt(selectionCount);
            int sqr = (int) Mathf.Floor(sqrfloat);

            var mazeBounds = maze.GetBounds();
            float horizontalDistance = mazeBounds.size.x / (float) sqr;
            float halfHorizontalDistance = horizontalDistance / 2;
            float verticalDistance = mazeBounds.size.y / (float) sqr;
            float halfVerticalDistance = verticalDistance / 2;
            float halfMazeWidth = mazeBounds.size.x / 2.0f;
            float halfMazeHeight = mazeBounds.size.y / 2.0f;

            for (int i = 0; i < sqr; i++)
            {
                for (int j = 0; j < sqr; j++)
                {
                    float centroidPositionX = i * horizontalDistance + halfHorizontalDistance - halfMazeWidth;
                    float centroidPositionY = j * verticalDistance + halfVerticalDistance - halfMazeHeight;

                    float minSqrDistance = Mathf.Infinity;
                    Vector3Int closestPoint = Vector3Int.zero;
                    bool finded = false;
                    foreach (var point in points)
                    {
                        float diffX = point.x - centroidPositionX;
                        float diffY = point.y - centroidPositionY;

                        float sqrDist = diffX * diffX + diffY * diffY;
                        if (sqrDist < minSqrDistance)
                        {
                            minSqrDistance = sqrDist;
                            closestPoint = point;
                            finded = true;
                        }
                    }

                    if (finded && !selectedPoints.Contains(closestPoint))
                    {
                        selectedPoints.Add(closestPoint);
                        points.Remove(closestPoint);
                    }
                }
            }

            int leftToAdd = selectionCount - selectedPoints.Count;
            while (leftToAdd > 0 && points.Count > 0)
            {
                int randomIndex = Random.Range(0, points.Count);
                var randomPoint = points[randomIndex];
                selectedPoints.Add(randomPoint);
                points.RemoveAt(randomIndex);
                leftToAdd--;
            }

            return selectedPoints;
        }
    }
}