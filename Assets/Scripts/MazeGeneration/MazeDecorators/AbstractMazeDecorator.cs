using UnityEngine;

namespace MazeGeneration.MazeDecorators
{
    public abstract class AbstractMazeDecorator : ScriptableObject
    {
        /// <summary>
        /// Add decorative and environmental objects to a maze
        /// </summary>
        /// <param name="maze"></param> maze to decorate.
        public abstract void DecorateMaze(Maze maze);
    }
}
