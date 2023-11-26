using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Components;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class SetTargetUnitToClosestEnemyIfNotSet : BehaviourTreeLogicNode
    {
        private readonly Configuration _configuration;

        public SetTargetUnitToClosestEnemyIfNotSet(BehaviourTreeProperties properties) : base(properties)
        {
            _configuration = Context.Get<Configuration>();
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            if (context.TargetUnit != null)
            {
                return UniTask.FromResult(BehaviourTreeStatus.Success);
            }

            var colliders = Physics2D.OverlapCircleNonAlloc(context.Owner.transform.position, 10f, ColliderCache, _configuration.UnitMask);

            for (var i = 0; i < colliders; i++)
            {
                var player = ColliderCache[i].gameObject.GetComponent<PlayerComponent>();

                if (player == null)
                {
                    continue;
                }

                context.TargetUnit = ColliderCache[i].gameObject;
                return UniTask.FromResult(BehaviourTreeStatus.Success);
            }

            return UniTask.FromResult(BehaviourTreeStatus.Failure);
        }
    }
}
