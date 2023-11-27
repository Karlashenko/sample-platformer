using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components.Entities;

namespace Sample.AI.Tasks
{
    public class SetTargetPointToSpawnPoint : BehaviourTreeNode
    {
        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var spawn = context.Owner.GetComponent<SpawnPointComponent>();

            if (!spawn)
            {
                return UniTask.FromResult(BehaviourTreeStatus.Failure);
            }

            context.TargetPoint = spawn.SpawnPoint;
            return UniTask.FromResult(BehaviourTreeStatus.Success);
        }
    }
}
