using Sample.Abilities;
using Sample.Values;
using UnityEngine;

namespace Sample.Components
{
    public class MonsterComponent : Component
    {
        [SerializeField] private AbilitiesComponent _abilities = null!;
        [SerializeField] private float _attackDelay;
        [SerializeField] private float _attackDuration;

        private void Start()
        {
            _abilities.Add(new MeleeAttackAbility(gameObject,
                new MeleeAttackAbilityParameters(new Damage(1, DamageType.Slashing), new Vector2(3, 2), 2, "Attack", _attackDelay, _attackDuration)));
        }
    }
}
