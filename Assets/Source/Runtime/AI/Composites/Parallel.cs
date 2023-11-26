using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Composites
{
    public class Parallel : BehaviourTreeNodeContainer
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var succeeded = 0;

            foreach (var node in Nodes)
            {
                var status = await node.Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Failure)
                {
                    return status;
                }

                if (status == BehaviourTreeStatus.Success)
                {
                    succeeded += 1;
                }
            }

            return succeeded == Nodes.Count ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;
        }
    }
}