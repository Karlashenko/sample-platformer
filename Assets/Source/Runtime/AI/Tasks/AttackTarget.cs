using System.Threading;
using Cysharp.Threading.Tasks;
using Sample.Abilities;
using Sample.Components;

namespace Sample.AI.Tasks
{
    public class AttackTarget : BehaviourTreeLogicNode
    {
        private IAbility _ability = null!;

        public AttackTarget(BehaviourTreeProperties properties) : base(properties)
        {
        }

        protected override void OnStart(BehaviourTreeContext context)
        {
            _ability = context.Owner.GetComponent<AbilitiesComponent>().Get<MeleeAttackAbility>();
            _ability.Use().Forget();
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            var status = _ability.IsRunning ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;
            return UniTask.FromResult(status);
        }
    }
}
