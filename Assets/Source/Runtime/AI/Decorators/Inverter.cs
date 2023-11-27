using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Decorators
{
    public class Inverter : BehaviourTreeCompositeNode
    {
        public Inverter()
        {
        }

        public Inverter(BehaviourTreeNode node)
        {
            AddNode(node);
        }

        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var status = await Nodes[0].Tick(deltaTime, context, cancellationToken);

            if (status == BehaviourTreeStatus.Failure)
            {
                return BehaviourTreeStatus.Success;
            }

            if (status == BehaviourTreeStatus.Success)
            {
                return BehaviourTreeStatus.Failure;
            }

            return status;
        }

        public virtual void Open()
        {
        }
    }
}
