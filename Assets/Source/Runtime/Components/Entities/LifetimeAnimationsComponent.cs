using UnityEngine;

namespace Sample.Components.Entities
{
    public class LifetimeAnimationsComponent : MonoBehaviour
    {
        private Animator _animator = null!;
        private HealthComponent? _health;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponent<HealthComponent>();

            if (_health)
            {
                _health.Damaged += OnDamaged;
                _health.Died += OnDied;
            }
        }

        private void OnDestroy()
        {
            if (_health)
            {
                _health.Damaged -= OnDamaged;
                _health.Died -= OnDied;
            }
        }

        private void OnDamaged(DamageEvent payload)
        {
            _animator.Play("Hit");
        }

        private void OnDied(DeathEvent payload)
        {
            _animator.Play("Death");
        }
    }
}
