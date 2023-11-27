using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Composites
{
    public class Parallel : BehaviourTreeCompositeNode
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var succeeded = 0;
            var failed = 0;

            foreach (var node in Nodes)
            {
                var status = await node.Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Success)
                {
                    succeeded += 1;
                }
                else if (status == BehaviourTreeStatus.Failure)
                {
                    failed += 1;
                }
            }

            if (failed == 0)
            {
                return BehaviourTreeStatus.Success;
            }

            if (succeeded + failed == Nodes.Count)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Running;
        }
    }
}
