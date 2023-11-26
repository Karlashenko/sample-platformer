using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FollowTargetKamikaze : BehaviourTreeLogicNode
    {
        private Transform _owner;
        private Transform _target;
        private MovementComponent _movement;

        public FollowTargetKamikaze(BehaviourTreeProperties properties) : base(properties)
        {
        }

        protected override void OnStart(BehaviourTreeContext context)
        {
            _owner = context.Owner.transform;
            _target = context.TargetUnit.transform;
            _movement = context.Owner.GetComponent<MovementComponent>();
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            const float maxSpeed = 12.0f;
            const float maxSteer = 0.03f;

            var direction = (Vector2) (_target.position - _owner.position).normalized;
            var desired = direction * maxSpeed;
            var steering = Vector2.ClampMagnitude(desired - _movement.GetVelocity(), maxSteer);

            _movement.AddVelocity(steering);

            return UniTask.FromResult((_target.position - _owner.position).sqrMagnitude < 2
                ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running);

        }
    }
}
