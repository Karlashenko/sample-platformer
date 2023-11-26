using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sample.Components;
using Sample.Utils;
using Sample.Values;
using UnityEngine;

namespace Sample.Abilities
{
    public class MeleeAttackAbility : Ability
    {
        private const int MaxTargets = 5;

        private static readonly Collider2D[] _hits = new Collider2D[MaxTargets];

        private readonly GameObject _owner;
        private readonly MeleeAttackAbilityParameters _parameters;
        private readonly MovementComponent? _movement;
        private readonly InputComponent? _input;
        private readonly Animator? _animator;
        private readonly Bounds _bounds;
        private readonly Configuration _configuration;

        public MeleeAttackAbility(GameObject owner, MeleeAttackAbilityParameters parameters) : base(0)
        {
            _owner = owner;
            _parameters = parameters;

            _movement = _owner.GetComponent<MovementComponent>();
            _input = _owner.GetComponent<InputComponent>();
            _animator = _owner.GetComponentInChildren<Animator>();
            _bounds = _owner.GetComponent<BoxCollider2D>().bounds;
            _configuration = Context.Get<Configuration>();
        }

        protected override async UniTask OnUse(CancellationToken cancellationToken = default(CancellationToken))
        {
            _animator?.Play(_parameters.Animation);

            _input?.ResetState();
            _input?.Disable();

            {
                await UniTask.Delay((int) (_parameters.AttackDelay * 1000), cancellationToken: cancellationToken);

                var epicenter = _bounds.center + new Vector3(_parameters.Range * _movement?.GetDirection().x ?? 0, 0);
                var size = Physics2D.OverlapBox(epicenter, _parameters.Area, 0, _configuration.DamageContactFilter, _hits);

                for (var i = 0; i < Math.Clamp(size, 0, MaxTargets); i++)
                {
                    var target = _hits[i];

                    if (!target.gameObject.IsEnemyOf(_owner))
                    {
                        continue;
                    }

                    var health = target.GetComponent<HealthComponent>();

                    if (health)
                    {
                        health.Damage(_parameters.Damage, _owner);
                    }
                }

                await UniTask.Delay((int) (_parameters.AttackDuration * 1000), cancellationToken: cancellationToken);

                #if UNITY_EDITOR
                DebugUtils.DrawRect(epicenter, _parameters.Area, Color.red);
                #endif
            }

            _input?.Enable();
        }
    }

    [Serializable]
    public class MeleeAttackAbilityParameters
    {
        public readonly Damage Damage;
        public readonly Vector2 Area;
        public readonly float Range;
        public readonly string Animation;
        public readonly float AttackDelay;
        public readonly float AttackDuration;

        public MeleeAttackAbilityParameters(Damage damage, Vector2 area, float range, string animation, float attackDelay, float attackDuration)
        {
            Damage = damage;
            Area = area;
            Range = range;
            Animation = animation;
            AttackDelay = attackDelay;
            AttackDuration = attackDuration;
        }
    }
}
