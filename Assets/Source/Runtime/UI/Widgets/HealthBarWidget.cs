using Sample.Components;
using Sample.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sample.UI.Widgets
{
    public class HealthBarWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText = null!;
        [SerializeField] private Image _fillImage = null!;
        [SerializeField] private RectTransform _container = null!;

        private HealthComponent _health;
        private Camera _camera;

        public void Construct(HealthComponent health, Camera camera)
        {
            _health = health;
            _camera = camera;

            _health.Damaged += OnHealthDamaged;
            _health.Healed += OnHealthHealed;
            _health.Died += OnHealthDied;

            _fillImage.color = health.GetComponent<PlayerComponent>() ? Color.green : Color.red;

            Refresh();
        }

        public void Deconstruct()
        {
            _health.Damaged -= OnHealthDamaged;
            _health.Healed -= OnHealthHealed;
            _health.Died -= OnHealthDied;

            Destroy(gameObject);
        }

        private void Refresh()
        {
            if (_health.Maximum < 10)
            {
                _container.localScale = _container.localScale.With(x: 0.5f);
            }

            _amountText.text = _health.Current.ToString();
            _fillImage.fillAmount = (float) _health.Current / _health.Maximum;
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (_health == null)
            {
                Deconstruct();
                return;
            }
            #endif

            transform.position = _camera.WorldToScreenPoint(_health.transform.position + new Vector3(0, 1));
        }

        private void OnHealthDamaged(DamageEvent payload)
        {
            Refresh();
        }

        private void OnHealthHealed(HealingEvent payload)
        {
            Refresh();
        }

        private void OnHealthDied(DeathEvent payload)
        {
            Deconstruct();
        }
    }
}
