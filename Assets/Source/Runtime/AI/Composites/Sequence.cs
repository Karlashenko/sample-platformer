using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Extensions;

namespace Sample.AI.Composites
{
    public class Sequence : BehaviourTreeCompositeNode
    {
        public Sequence(params BehaviourTreeNode[] nodes)
        {
            Nodes.AddRange(nodes);
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var index = context.RunningNodeIndices.GetValueOrDefault(this, 0);

            for (var i = index; i < Nodes.Count; i++)
            {
                var status = await Nodes[i].Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Success)
                {
                    continue;
                }

                if (status == BehaviourTreeStatus.Running)
                {
                    context.RunningNodeIndices[this] = i;
                }

                return status;
            }

            return BehaviourTreeStatus.Success;
        }

        protected override void OnStop(BehaviourTreeContext context)
        {
            context.RunningNodeIndices[this] = 0;
        }
    }
}
