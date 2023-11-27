using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components.Entities;
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
        private readonly Damage _damage;
        private readonly MeleeAttackAbilityParameters _parameters;
        private readonly MovementComponent? _movement;
        private readonly InputComponent? _input;
        private readonly Animator? _animator;
        private readonly BoxCollider2D _boxCollider;
        private readonly Configuration _configuration;

        public MeleeAttackAbility(GameObject owner, Damage damage, MeleeAttackAbilityParameters parameters) : base(parameters.Cooldown)
        {
            _owner = owner;
            _damage = damage;
            _parameters = parameters;

            _movement = _owner.GetComponent<MovementComponent>();
            _input = _owner.GetComponent<InputComponent>();
            _animator = _owner.GetComponentInChildren<Animator>();
            _boxCollider = _owner.GetComponent<BoxCollider2D>();
            _configuration = Context.Get<Configuration>();
        }

        protected override async UniTask OnUse(CancellationToken cancellationToken = default(CancellationToken))
        {
            _input?.ResetState();
            _input?.Disable();

            _animator?.SetBool("Mute", true);
            _animator?.Play(_parameters.Animation);

            {
                await UniTask.Delay(_parameters.AttackDelay, cancellationToken: cancellationToken);

                var epicenter = _boxCollider.bounds.center + new Vector3(_parameters.Range * _movement?.GetDirection().x ?? 0, 0);
                var size = Physics2D.OverlapBox(epicenter, _parameters.Area, 0, _configuration.DamageContactFilter, _hits);

                #if UNITY_EDITOR
                DebugUtils.DrawRect(epicenter, _parameters.Area, Color.red);
                #endif

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
                        health.Damage(_damage, _owner);
                    }
                }

                await UniTask.Delay(_parameters.AttackDuration, cancellationToken: cancellationToken);
            }

            _animator?.SetBool("Mute", false);
            _input?.Enable();
        }
    }

    [Serializable]
    public class MeleeAttackAbilityParameters
    {
        public Vector2 Area;
        public float Range;
        public string Animation;
        public int AttackDelay;
        public int AttackDuration;
        public int Cooldown;

        public MeleeAttackAbilityParameters(Vector2 area, float range, string animation, int attackDelay, int attackDuration, int cooldown)
        {
            Area = area;
            Range = range;
            Animation = animation;
            AttackDelay = attackDelay;
            AttackDuration = attackDuration;
            Cooldown = cooldown;
        }
    }
}
