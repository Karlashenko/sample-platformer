using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Abilities;
using Sample.Components.Entities;

namespace Sample.AI.Tasks
{
    public class AttackTarget : BehaviourTreeNode
    {
        private IAbility _ability = null!;

        public AttackTarget()
        {
        }

        protected override bool CanStart(BehaviourTreeContext context)
        {
            _ability = context.Owner.GetComponent<AbilitiesComponent>().Get<MeleeAttackAbility>();

            if (_ability.IsOnCooldown || _ability.IsRunning)
            {
                return false;
            }

            _ability.Use().Forget();
            return true;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var status = _ability.IsRunning ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
            return UniTask.FromResult(status);
        }
    }
}
