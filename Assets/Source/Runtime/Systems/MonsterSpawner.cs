using System.Linq;
using Sample.Components.Entities;
using UnityEngine;

namespace Sample.Systems
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private MonsterComponent _prefab = null!;
        [SerializeField] private Transform[] _points = null!;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerComponent>())
            {
                return;
            }

            foreach (var point in _points)
            {
                var position = point.position;
                var monster = Instantiate(_prefab, position, Quaternion.identity);

                monster.gameObject.AddComponent<SpawnPointComponent>().Construct(position);

                Debug.Log($"Spawned {_prefab.name} at {position}");
            }

            gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            Refresh();
        }

        public void Refresh()
        {
            _points = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }
    }
}
