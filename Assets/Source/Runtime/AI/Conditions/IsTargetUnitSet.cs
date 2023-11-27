using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Conditions
{
    public class IsTargetUnitSet : BehaviourTreeNode
    {
        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var status = context.TargetUnit == null
                ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Success;

            return UniTask.FromResult(status);
        }
    }
}
