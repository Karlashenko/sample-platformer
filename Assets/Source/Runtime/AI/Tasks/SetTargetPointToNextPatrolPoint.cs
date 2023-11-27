using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components.Entities;

namespace Sample.AI.Tasks
{
    public class SetTargetPointToNextPatrolPoint : BehaviourTreeNode
    {
        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var patrol = context.Owner.GetComponent<PatrolAreaComponent>();

            if (!patrol)
            {
                return UniTask.FromResult(BehaviourTreeStatus.Failure);
            }

            context.TargetPoint = patrol.GetNextPoint();

            return UniTask.FromResult(BehaviourTreeStatus.Success);
        }
    }
}
