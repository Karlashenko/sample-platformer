using Sample.Components;
using Sample.UI.Widgets;
using UnityEngine;

namespace Sample.Systems
{
    public class HealthBarSystem : MonoBehaviour
    {
        [SerializeField] private HealthBarWidget _prefab;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;

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
