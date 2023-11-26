using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Conditions
{
    public class IsTargetPointReachable : BehaviourTreeLogicNode
    {
        public IsTargetPointReachable(BehaviourTreeProperties properties) : base(properties)
        {
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(BehaviourTreeStatus.Success);
        }
    }
}
