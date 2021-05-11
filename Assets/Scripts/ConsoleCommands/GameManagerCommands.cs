using IngameDebugConsole;

namespace ConsoleCommands
{
    public class GameManagerCommands
    {
        [ConsoleMethod("maze.gen_puzzle", "Generates a maze with one puzzle by index:\n" +
                                          "0: Blood sacrifice\n" +
                                          "1: Combinations\n" +
                                          "2: Door and key\n" +
                                          "3: King\n" +
                                          "4: Levers\n" +
                                          "5: Musical\n" +
                                          "6: Numbers sequence\n" +
                                          "7: Scattered code\n" +
                                          "8: Spiders")]
        public static void GenerateWithPuzzles(int index=-1)
        {
            GameManager.Instance.GenerateMazeWithPuzzleIndex(index);
        }

        [ConsoleMethod("maze.restart_level", "Restarts the level")]
        public static void RestartLevel()
        {
            GameManager.Instance.RestartLevel();
        }
    }
}