using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FollowTargetJetpack : BehaviourTreeLogicNode
    {
        private Transform _owner;
        private Transform _target;
        private MovementComponent _movement;

        public FollowTargetJetpack(BehaviourTreeProperties properties) : base(properties)
        {
        }

        protected override void OnStart(BehaviourTreeContext context)
        {
            _owner = context.Owner.transform;
            _target = context.TargetUnit.transform;
            _movement = context.Owner.GetComponent<MovementComponent>();
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            const float maxSpeed = 12.0f;
            const float maxSteer = 0.03f;
            const float angularSpeed = 8f;

            var approachRange = Properties.FollowTargetStopDistance;
            var sqrApproachRange = approachRange * approachRange;

            var sqrMagnitude = (_target.position - _owner.position).sqrMagnitude;
            var speedFactor = Mathf.Min(sqrMagnitude / sqrApproachRange, 1);

            var direction = (Vector2) (_target.position - _owner.position).normalized;
            var angular = Vector2.Perpendicular(direction) * angularSpeed;
            var desired = direction * (maxSpeed * speedFactor) + angular * Mathf.Sign(direction.x);

            var steering = Vector2.ClampMagnitude(desired - _movement.GetVelocity(), maxSteer);

            _movement.AddVelocity(steering);

            return BehaviourTreeStatus.Running;
        }
    }
}
