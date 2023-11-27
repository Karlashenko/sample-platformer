using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Tasks
{
    public class Break : BehaviourTreeNode
    {
        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(BehaviourTreeStatus.Failure);
        }
    }
}
