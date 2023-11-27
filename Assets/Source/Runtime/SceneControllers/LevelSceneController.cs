using Cysharp.Threading.Tasks;
using Sample.Components.Entities;
using Sample.Pathfinding;
using Sample.Systems;
using Sample.UI.Screens;
using UnityEngine;

namespace Sample.SceneControllers
{
    public class LevelSceneController : MonoBehaviour
    {
        [SerializeField] private PauseScreen _pauseScreen = null!;
        [SerializeField] private PlayerComponent _playerPrefab = null!;
        [SerializeField] private Transform _playerSpawn = null!;

        private PlayerComponent _player = null!;

        private void Awake()
        {
            Context.Set(FindFirstObjectByType<Pathfinder>());
            Context.Set(FindFirstObjectByType<CameraFollow>());
            Context.Set(FindFirstObjectByType<PathfinderTest>());
            Context.Set(FindFirstObjectByType<Configuration>());
            Context.Set(FindFirstObjectByType<Waypoints>());
        }

        private void Start()
        {
            SpawnPlayer().Forget();
        }

        private void OnDestroy()
        {
            Context.Clear();
        }

        private void Update()
        {
            if (KeyBindings.IsKeyPressed(InputKey.Menu))
            {
                TogglePauseScreen();
            }
        }

        private void TogglePauseScreen()
        {
            var isPauseScreenActive = !_pauseScreen.gameObject.activeSelf;
            _pauseScreen.gameObject.SetActive(isPauseScreenActive);

            if (isPauseScreenActive)
            {
                Game.Pause();
            }
            else
            {
                Game.Unpause();
            }
        }

        private async UniTask SpawnPlayer(int millisecondsDelay = 0)
        {
            await UniTask.Delay(millisecondsDelay);

            _player = Instantiate(_playerPrefab, _playerSpawn.position, Quaternion.identity);
            _player.GetComponent<HealthComponent>().Died += OnPlayerDied;
            Context.Get<CameraFollow>().SetTarget(_player.transform);
        }

        private void OnPlayerDied(DeathEvent payload)
        {
            _player.GetComponent<HealthComponent>().Died -= OnPlayerDied;
            Context.Get<CameraFollow>().UnsetTarget();

            Restart();
        }

        private void Restart()
        {
            SpawnPlayer(3000).Forget();
            RespawnMonsters();
        }

        private static void RespawnMonsters()
        {
            foreach (var monster in FindObjectsByType<MonsterComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                Destroy(monster.gameObject);
            }

            foreach (var spawner in FindObjectsByType<MonsterSpawner>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                spawner.gameObject.SetActive(true);
            }
        }
    }
}
