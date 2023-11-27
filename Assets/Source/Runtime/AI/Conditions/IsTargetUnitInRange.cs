using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Conditions
{
    public class IsTargetUnitInRange : BehaviourTreeNode
    {
        private readonly float _range;

        public IsTargetUnitInRange(float range)
        {
            _range = range;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var sqrMagnitude = (context.TargetUnit.transform.position - context.Owner.transform.position).sqrMagnitude;
            var status = sqrMagnitude < _range * _range ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
            return UniTask.FromResult(status);
        }
    }
}
