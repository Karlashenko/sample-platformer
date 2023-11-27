using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sample
{
    public static class Game
    {
        public static bool IsPaused { get; private set; }

        public static void ToMainMenuScene()
        {
            SceneManager.LoadScene(0);
        }

        public static void ToLevelScene()
        {
            SceneManager.LoadScene(1);
        }

        public static void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0;
        }

        public static void Unpause()
        {
            IsPaused = false;
            Time.timeScale = 1;
        }
    }
}
