using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Composites
{
    public class Race : BehaviourTreeNodeContainer
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var failed = 0;

            foreach (var node in Nodes)
            {
                var status = await node.Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Success)
                {
                    return status;
                }

                if (status == BehaviourTreeStatus.Failure)
                {
                    failed += 1;
                }
            }

            return failed == Nodes.Count ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Running;
        }
    }
}
