using MazeGeneration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor
{
    public static class MazeGenerationEditor
    {
        [MenuItem("Mazes/GenerateMaze %g")]
        public static void GenerateMaze()
        {
            var mazeController = Object.FindObjectOfType<MazeController>();
            mazeController.GenerateMaze();
        } 
        
        [MenuItem("Mazes/GenerateMazeWithPuzzles #&g")]
        public static void GenerateMazeWithPuzzles()
        {
            var gameManager = Object.FindObjectOfType<GameManager>();
            gameManager.GenerateMazeWithNewPuzzles();
        }

        [MenuItem("Assets/Romb Tider/Set tile as floor", false)]
        public static void SetAsFloor()
        {
            var maze = Object.FindObjectOfType<Maze>();
            var selectedTile = Selection.GetFiltered<TileBase>(SelectionMode.Assets)[0];
            
            Debug.Log(maze);
            Debug.Log(selectedTile);
            
            maze.FloorSpriteTile = selectedTile;
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("Assets/Romb Tider/Set tile as wall", false)]
        public static void SetAsWall()
        {
            var maze = Object.FindObjectOfType<Maze>();
            var selectedTile = Selection.GetFiltered<TileBase>(SelectionMode.Assets)[0];
            
            Debug.Log(maze);
            Debug.Log(selectedTile);
            
            maze.WallSpriteTile = selectedTile;
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("Assets/Romb Tider/Set tile as floor", true)]
        [MenuItem("Assets/Romb Tider/Set tile as wall", true)]
        public static bool SetTileValidator()
        {
            var tiles = Selection.GetFiltered<TileBase>(SelectionMode.Assets);
            var onlyOneSelected = tiles.Length == 1;

            return onlyOneSelected;
        }
    }
     
}
