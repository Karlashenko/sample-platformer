using UnityEngine;
using UnityEngine.UI;

namespace Sample.UI.Screens
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _playButton = null!;
        [SerializeField] private Button _exitButton = null!;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            Game.ToLevelScene();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
