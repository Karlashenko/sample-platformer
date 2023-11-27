using System;
using Cysharp.Threading.Tasks;
using Sample.Values;
using UnityEngine;

namespace Sample.Components.Entities
{
    public class HealthComponent : Component
    {
        public static event Action<HealthComponent>? AnyHealthComponentCreated;

        public event Action<DamageEvent>? Damaged;
        public event Action<HealingEvent>? Healed;
        public event Action<DeathEvent>? Died;

        public bool IsAlive => Current > 0;
        public int Current { get; private set; }
        public int Maximum => _maxHealth;

        [SerializeField] private int _maxHealth;

        private void Start()
        {
            Current = Maximum;
            AnyHealthComponentCreated?.Invoke(this);
        }

        public void Damage(Damage damage, GameObject attacker)
        {
            if (!IsAlive)
            {
                Debug.LogWarning($"'{attacker.name}' is trying to attack '{gameObject.name}' while he's dead.");
                return;
            }

            Current = Math.Max(Current - damage.Amount, 0);
            Damaged?.Invoke(new DamageEvent(damage, attacker, gameObject));

            if (Current == 0)
            {
                Died?.Invoke(new DeathEvent(damage, attacker, gameObject));
                DeathRoutine().Forget();
            }
        }

        public void Heal(Healing healing, GameObject healer)
        {
            Current = Math.Min(Current + healing.Amount, _maxHealth);
            Healed?.Invoke(new HealingEvent(healing, healer, gameObject));
        }

        private async UniTask DeathRoutine()
        {
            GetComponent<BehaviourTreeComponent>()?.Disable();

            var inputComponent = GetComponent<InputComponent>();
            inputComponent?.ResetState();
            inputComponent?.Disable();

            var movement = GetComponent<MovementComponent>();
            movement?.EnableCollision();
            movement?.SetVelocity(new Vector2(0, -5));

            await UniTask.Delay(2000);

            Destroy(gameObject);
        }
    }

    public readonly struct DamageEvent
    {
        public readonly Damage Damage;
        public readonly GameObject Attacker;
        public readonly GameObject Victim;

        public DamageEvent(Damage damage, GameObject attacker, GameObject victim)
        {
            Damage = damage;
            Attacker = attacker;
            Victim = victim;
        }
    }

    public readonly struct DeathEvent
    {
        public readonly Damage Damage;
        public readonly GameObject Killer;
        public readonly GameObject Victim;

        public DeathEvent(Damage damage, GameObject killer, GameObject victim)
        {
            Damage = damage;
            Killer = killer;
            Victim = victim;
        }
    }

    public readonly struct HealingEvent
    {
        public readonly Healing Healing;
        public readonly GameObject Healer;
        public readonly GameObject Target;

        public HealingEvent(Healing healing, GameObject healer, GameObject target)
        {
            Healing = healing;
            Healer = healer;
            Target = target;
        }
    }
}
