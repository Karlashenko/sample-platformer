using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Composites
{
    public class Tree : BehaviourTreeNodeContainer
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            foreach (var node in Nodes)
            {
                await node.Tick(deltaTime, context, cancellationToken);
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
