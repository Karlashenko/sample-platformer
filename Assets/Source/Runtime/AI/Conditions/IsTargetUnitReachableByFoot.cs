using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Pathfinding;

namespace Sample.AI.Conditions
{
    public class IsTargetUnitReachableByFoot : BehaviourTreeNode
    {
        private readonly Pathfinder _pathfinder;

        public IsTargetUnitReachableByFoot()
        {
            _pathfinder = Context.Get<Pathfinder>();
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var path = await _pathfinder.FindPath(context.Owner.transform.position, context.TargetUnit!.transform.position, cancellationToken);
            var status = path == null ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Success;

            path?.Dispose();

            return status;
        }
    }
}
