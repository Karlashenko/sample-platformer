using Sample.Abilities;
using Sample.Values;
using UnityEngine;

namespace Sample.Components.Entities
{
    public class MonsterComponent : Component
    {
        [SerializeField] private AbilitiesComponent _abilities = null!;
        [SerializeField] private MeleeAttackAbilityParameters _attackParameters;

        private void Start()
        {
            _abilities.Add(new MeleeAttackAbility(gameObject, new Damage(1, DamageType.Slashing), _attackParameters));
        }
    }
}
