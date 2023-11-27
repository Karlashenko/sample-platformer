using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Tasks
{
    public class ClearTargetUnit : BehaviourTreeNode
    {
        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            context.TargetUnit = null;
            return UniTask.FromResult(BehaviourTreeStatus.Success);
        }
    }
}
