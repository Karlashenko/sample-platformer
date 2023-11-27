using Sample.Components.Entities;
using Sample.UI.Widgets;
using UnityEngine;

namespace Sample.Systems
{
    public class HealthBarSystem : MonoBehaviour
    {
        [SerializeField] private HealthBarWidget _prefab = null!;
        [SerializeField] private Canvas _canvas = null!;
        [SerializeField] private Camera _camera = null!;

        private void Awake()
        {
            HealthComponent.AnyHealthComponentCreated += OnAnyHealthComponentCreated;
        }

        private void OnAnyHealthComponentCreated(HealthComponent health)
        {
            Instantiate(_prefab, _canvas.transform).Construct(health, _camera);
        }
    }
}
