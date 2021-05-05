using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public static class MazePaths
    {
        public static void GetDfsPath(Tilemap maze, List<Vector3Int> path, Vector3Int from, Vector3Int to)
        {
            path.Clear();
            var visited = new List<Vector3Int>();
            var neighbours = new List<Vector3Int>();
            var toVisitList = new LinkedList<(Vector3Int, int)>();
            toVisitList.AddFirst((from, 0));
            
            if (!maze.HasTile(from))
            {
                Debug.Log("!!FROM not in MAZE!!!!!");
            }
            
            while (toVisitList.Count > 0)
            {
                (var checkPosition, int count) = toVisitList.First.Value;
                toVisitList.RemoveFirst();
                visited.Add(checkPosition);

                while (path.Count > count)
                {
                    path.RemoveAt(count);
                }
                
                path.Add(checkPosition);

                if (checkPosition == to)
                {
                    break;  // path found
                }
                
                GetNeighbours(maze, checkPosition, neighbours);
//                SortByManhattanDist(neighbours, to);

                foreach (var neighbour in neighbours)
                {
                    bool notVisited = !visited.Contains(neighbour);
                    if (notVisited)
                    {
                        toVisitList.AddFirst((neighbour, count + 1));
                    }
                }
            }
        }
        
        private static void GetNeighbours(Tilemap maze, Vector3Int tilePosition, List<Vector3Int> neighbours)
        {
            neighbours.Clear();

            var topNeighbour = tilePosition + Vector3Int.up;
            if (maze.HasTile(topNeighbour))
            {
                neighbours.Add(topNeighbour);
            }
            
            var bottomNeighbour = tilePosition + Vector3Int.down;
            if (maze.HasTile(bottomNeighbour))
            {
                neighbours.Add(bottomNeighbour);
            }
            
            var leftNeighbour = tilePosition + Vector3Int.left;
            if (maze.HasTile(leftNeighbour))
            {
                neighbours.Add(leftNeighbour);
            }
            
            var rightNeighbour = tilePosition + Vector3Int.right;
            if (maze.HasTile(rightNeighbour))
            {
                neighbours.Add(rightNeighbour);
            }
        }

        private static void SortByManhattanDist(List<Vector3Int> positions, Vector3Int target)
        {
            var comparablePositions = new List<Vector3IntManhattanDistComparable>();
            foreach (var position in positions)
            {
                var comparable = new Vector3IntManhattanDistComparable(position, target);
                comparablePositions.Add(comparable);
            }
            comparablePositions.Sort();
            positions.Clear();
            foreach (var position in comparablePositions)
            {
                positions.Add(position.Self);
            }
        }
    }
    
    internal class Vector3IntManhattanDistComparable : IComparable
    {
        private readonly Vector3Int _self;
        private readonly int _manhattanDist;
        
        public Vector3IntManhattanDistComparable(Vector3Int self, Vector3Int target)
        {
            _self = self;
            var diff = target - self;
            _manhattanDist = Math.Abs(diff.x) + Math.Abs(diff.y);
        }

        public Vector3Int Self => _self;

        public int CompareTo(object obj)
        {
            return ((Vector3IntManhattanDistComparable) obj)._manhattanDist.CompareTo(_manhattanDist);
        }
    }
}