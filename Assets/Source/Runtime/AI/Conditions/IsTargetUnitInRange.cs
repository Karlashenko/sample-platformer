using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Conditions
{
    public class IsTargetUnitInRange : BehaviourTreeLogicNode
    {
        public IsTargetUnitInRange(BehaviourTreeProperties properties) : base(properties)
        {
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var sqrMagnitude = (context.TargetUnit.transform.position - context.Owner.transform.position).sqrMagnitude;
            var status = sqrMagnitude < Properties.Range * Properties.Range ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
            return UniTask.FromResult(status);
        }
    }
}
