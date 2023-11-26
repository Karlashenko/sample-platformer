using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Decorators
{
    public class Succeeder : BehaviourTreeNodeContainer
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            if (Nodes.Count == 0)
            {
                throw new ApplicationException($"{GetType().Name} must have a child node!");
            }

            return await Nodes.First().Tick(deltaTime, context, cancellationToken) == BehaviourTreeStatus.Running
                ? BehaviourTreeStatus.Running
                : BehaviourTreeStatus.Success;
        }
    }
}
