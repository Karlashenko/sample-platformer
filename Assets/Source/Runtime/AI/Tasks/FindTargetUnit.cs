using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Utils;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FindTargetUnit : BehaviourTreeNode
    {
        private readonly float _range;
        private readonly Configuration _configuration;

        public FindTargetUnit(float range)
        {
            _range = range;
            _configuration = Context.Get<Configuration>();
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var count = Physics2D.OverlapCircle(context.Owner.transform.position, _range, _configuration.DamageContactFilter, ColliderCache);

            for (var i = 0; i < Math.Min(count, ColliderCache.Length); i++)
            {
                if (!ColliderCache[i].gameObject.IsEnemyOf(context.Owner))
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
