using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components;
using UnityEngine;

namespace Sample.Abilities
{
    public class DashAbility : Ability
    {
        private readonly GameObject _owner;
        private readonly Vector2 _impulse;

        public DashAbility(GameObject owner, Vector2 impulse) : base(1000)
        {
            _owner = owner;
            _impulse = impulse;
        }

        protected override UniTask OnUse(CancellationToken cancellationToken = default(CancellationToken))
        {
            var movement = _owner.GetComponent<MovementComponent>();

            movement.AddImpulse(new Vector2(_impulse.x * movement.GetDirection().x, _impulse.y));

            return UniTask.CompletedTask;
        }
    }
}
