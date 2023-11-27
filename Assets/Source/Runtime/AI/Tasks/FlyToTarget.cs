using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components.Entities;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public abstract class FlyToTarget : BehaviourTreeNode
    {
        private readonly float _stopDistance;

        private Transform _owner = null!;
        private MovementComponent _movement = null!;

        protected FlyToTarget(float stopDistance)
        {
            _stopDistance = stopDistance;
        }

        protected abstract Vector3 GetTargetPoint(BehaviourTreeContext context);

        protected override void OnStart(BehaviourTreeContext context)
        {
            _owner = context.Owner.transform;
            _movement = context.Owner.GetComponent<MovementComponent>();
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            const float maxSpeed = 12.0f;
            const float maxSteer = 0.03f;

            var targetPoint = GetTargetPoint(context);
            var direction = (Vector2) (targetPoint - _owner.position).normalized;
            var desired = direction * maxSpeed;
            var steering = Vector2.ClampMagnitude(desired - _movement.GetVelocity(), maxSteer);

            _movement.AddVelocity(steering);

            var status = (targetPoint - _owner.position).magnitude < _stopDistance
                ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;

            Debug.DrawLine(context.Owner.transform.position, targetPoint, Color.magenta);

            return UniTask.FromResult(status);

        }
    }
}
