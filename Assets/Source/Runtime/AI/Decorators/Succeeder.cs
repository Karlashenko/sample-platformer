using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Decorators
{
    public class Succeeder : BehaviourTreeCompositeNode
    {
        protected override async UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            return await Nodes[0].Tick(deltaTime, context, cancellationToken) == BehaviourTreeStatus.Running
                ? BehaviourTreeStatus.Running
                : BehaviourTreeStatus.Success;
        }
    }
}
