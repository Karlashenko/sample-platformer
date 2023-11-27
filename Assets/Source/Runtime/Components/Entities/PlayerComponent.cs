using Sample.Abilities;
using Sample.Values;
using UnityEngine;

namespace Sample.Components.Entities
{
    public class PlayerComponent : Component
    {
        [SerializeField] private InputComponent _input = null!;
        [SerializeField] private MovementComponent _movement = null!;
        [SerializeField] private AbilitiesComponent _abilities = null!;

        private void Start()
        {
            _abilities.Add(new DashAbility(gameObject, new Vector2(20, 10)));
            _abilities.Add(new MeleeAttackAbility(gameObject, new Damage(1, DamageType.Slashing), new MeleeAttackAbilityParameters(new Vector2(3, 2), 1, "Attack_Swing", 0, 500, 0)));
        }

        private async void Update()
        {
            var input = _input.GetState();

            _movement.SetInput(input);

            if (input.DashPressed)
            {
                await _abilities.Get<DashAbility>().Use();
            }

            if (input.Action0Pressed)
            {
                await _abilities.Get<MeleeAttackAbility>().Use();
            }
        }
    }
}
