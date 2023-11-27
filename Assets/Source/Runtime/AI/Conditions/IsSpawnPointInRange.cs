using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components.Entities;

namespace Sample.AI.Conditions
{
    public class IsSpawnPointInRange : BehaviourTreeNode
    {
        private readonly float _range;

        public IsSpawnPointInRange(float range)
        {
            _range = range;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var spawn = context.Owner.GetComponent<SpawnPointComponent>();

            if (!spawn)
            {
                return UniTask.FromResult(BehaviourTreeStatus.Success);
            }

            var status = (spawn.SpawnPoint - context.Owner.transform.position).magnitude < _range ?
                BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;

            return UniTask.FromResult(status);
        }
    }
}
