using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Extensions;

namespace Sample.AI.Composites
{
    public class Selector : BehaviourTreeCompositeNode
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var index = context.RunningNodeIndices.GetValueOrDefault(this, 0);

            for (var i = index; i < Nodes.Count; i++)
            {
                var status = await Nodes[i].Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Failure)
                {
                    continue;
                }

                if (status == BehaviourTreeStatus.Running)
                {
                    context.RunningNodeIndices[this] = i;
                }

                return status;
            }

            return BehaviourTreeStatus.Failure;
        }

        protected override void OnStop(BehaviourTreeContext context)
        {
            context.RunningNodeIndices[this] = 0;
        }
    }
}
