using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration.InterestPointDetection
{
    public class InteresPointsVisualizer : MonoBehaviour
    {
        [SerializeField] private Maze maze;
        [SerializeField] private Tilemap visualizerTilemap;
        [SerializeField] private List<DetectorTileBase> detectors;

        [NaughtyAttributes.Button]
        public void Clear()
        {
            visualizerTilemap.ClearAllTiles();
        }

        [NaughtyAttributes.Button]
        public void Visualize()
        {
            Clear();
            foreach (var detector in detectors)
            {
                if (detector.enabled)
                {
                    var points = detector.detector.GetInterestingPoints(maze);
                    foreach (var point in points)
                    {
                        visualizerTilemap.SetTile(point, detector.tile);
                    }
                }
            }
        }

        [NaughtyAttributes.Button]
        public void RegisterGenerationObservation()
        {
            maze.eventMazePostChanged += Visualize;
        }
        
        [NaughtyAttributes.Button]
        public void UnregisterGenerationObservation()
        {
            maze.eventMazePostChanged -= Visualize;
        }
    }

    [Serializable]
    public struct DetectorTileBase
    {
        public InterestPointDetector detector;
        public TileBase tile;
        public bool enabled;
    }
}
