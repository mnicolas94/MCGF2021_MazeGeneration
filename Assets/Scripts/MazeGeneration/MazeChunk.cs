using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public class MazeChunk
    {
        private bool[][] _floorWallsStructure;
        private TileBase[][] _floorDetails;
        private TileBase[][] _wallsDetails;
        private List<(Vector2Int, Room)> _rooms;

    }
}