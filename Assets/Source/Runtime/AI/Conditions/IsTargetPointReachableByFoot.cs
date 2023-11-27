using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Pathfinding;

namespace Sample.AI.Conditions
{
    public class IsTargetPointReachableByFoot : BehaviourTreeNode
    {
        private readonly Pathfinder _pathfinder;

        public IsTargetPointReachableByFoot()
        {
            _pathfinder = Context.Get<Pathfinder>();
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var path = await _pathfinder.FindPath(context.Owner.transform.position, context.TargetPoint!.Value, cancellationToken);
            var status = path == null ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Success;

            path?.Dispose();

            return status;
        }
    }
}
