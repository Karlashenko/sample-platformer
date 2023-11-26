using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components;
using Sample.Extensions;
using Sample.Movement;
using Sample.Pathfinding;
using Unity.Collections;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FollowTargetGround : BehaviourTreeLogicNode
    {
        private readonly Configuration _configuration;
        private readonly Pathfinder _pathfinder;
        private readonly PathfinderTest _pathfinderTest;

        private Transform _owner;
        private Transform _target;
        private InputState _input;
        private bool _isJumping;
        private int _pathNodeIndex;
        private NativeList<PathfinderNode>? _path;
        private MovementComponent _movement;

        public FollowTargetGround(BehaviourTreeProperties properties) : base(properties)
        {
            _configuration = Context.Get<Configuration>();
            _pathfinder = Context.Get<Pathfinder>();
            _pathfinderTest = Context.Get<PathfinderTest>();
        }

        protected override void OnStart(BehaviourTreeContext context)
        {
            _owner = context.Owner.transform;
            _target = context.TargetUnit.transform;
            _movement = context.Owner.GetComponent<MovementComponent>();

            _input = new InputState();
            _isJumping = false;
            _pathNodeIndex = 0;
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var distance = (_owner.position - _target.position).magnitude;

            if (distance <= Properties.FollowTargetStopDistance)
            {
                var direction = (_target.position - _owner.position).normalized;
                var hit = Physics2D.Raycast(_owner.position, direction, distance, _configuration.CollisionMask);

                if (!hit)
                {
                    return BehaviourTreeStatus.Success;
                }
            }

            var status = await FollowTarget(cancellationToken);

            _movement.SetInput(_input);

            return status;
        }

        protected override void OnStop(BehaviourTreeContext context)
        {
            _path?.Dispose();
            _path = null;

            if (_movement.MovementBehaviourType == MovementBehaviourType.Ground)
            {
                _movement.SetInput(new InputState());
            }
            else
            {
                _movement.AddImpulse(_movement.GetVelocity());
                _movement.SetVelocity(Vector2.zero);
            }
        }

        private async UniTask<BehaviourTreeStatus> FollowTarget(CancellationToken cancellationToken)
        {
            _input.MoveRight = false;
            _input.MoveLeft = false;
            _input.MoveUp = false;
            _input.MoveDown = false;

            var unitOrigin = _owner.GetComponent<BoxCollider2D>().bounds.GetBottomCenter();

            if (!_path.HasValue || _pathNodeIndex > 2 || _pathNodeIndex >= _path.Value.Length)
            {
                _path?.Dispose();
                _path = await _pathfinder.FindPath(unitOrigin, _target.position, cancellationToken);
                _pathNodeIndex = 0;

                if (_path == null)
                {
                    return BehaviourTreeStatus.Failure;
                }

                return BehaviourTreeStatus.Running;
            }

            var destination = _path.Value[_pathNodeIndex];

            var magnitude = (destination.Position - (Vector2) unitOrigin).magnitude;

            if (magnitude < 0.5f)
            {
                _pathNodeIndex += 1;
                return BehaviourTreeStatus.Running;
            }

            if (_isJumping && _movement.IsGrounded())
            {
                _isJumping = false;
                _path?.Dispose();
                _path = null;
                return BehaviourTreeStatus.Running;
            }

            if (destination.WaypointConnectionType == WaypointConnectionType.Jump && _movement.IsGrounded())
            {
                _isJumping = true;
                JumpTo(destination.Position);
                return BehaviourTreeStatus.Running;
            }

            if (!_isJumping)
            {
                _input.MoveRight = destination.Position.x >= unitOrigin.x;
                _input.MoveLeft = destination.Position.x < unitOrigin.x;
            }

            return BehaviourTreeStatus.Running;
        }

        private void JumpTo(Vector3 target)
        {
            var origin = _owner.GetComponent<BoxCollider2D>().bounds.GetBottomCenter();
            var height = Mathf.Max(1, target.y - origin.y) + 0.5f;

            var jumpVelocity = CalculateJumpVelocity(origin, target, height, _movement.GetGravity());

            _movement.SetVelocity(jumpVelocity);
        }

        private static Vector3 CalculateJumpVelocity(Vector3 origin, Vector3 target, float height, float gravity)
        {
            var delta = target - origin;
            var time = Mathf.Sqrt(2 * height / gravity) + Mathf.Sqrt(2 * Mathf.Abs(delta.y - height) / gravity);

            var velocityX = delta.x / time;
            var velocityY = Mathf.Sqrt(2 * gravity * height);

            return new Vector3(velocityX, velocityY);
        }

        public override void DrawGizmos()
        {
            if (!_path.HasValue)
            {
                return;
            }

            var capacity = _path.Value.Length - _pathNodeIndex - 1;

            if (capacity < 1)
            {
                return;
            }

            var temp = new NativeList<PathfinderNode>(capacity, Allocator.Temp);

            for (var i = _pathNodeIndex; i < _path.Value.Length; i++)
            {
                temp.Add(_path.Value[i]);
            }

            _pathfinderTest.GizmoDrawPath(temp);
        }
    }
}
