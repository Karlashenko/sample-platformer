using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Decorators
{
    public class Inverter : BehaviourTreeNodeContainer
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            if (Nodes.Count == 0)
            {
                throw new Exception($"{GetType().Name} must have a child node!");
            }

            var status = await Nodes.First().Tick(deltaTime, context, cancellationToken);

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
