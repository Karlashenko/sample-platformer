using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI.Tasks
{
    public class Wait : BehaviourTreeLogicNode
    {
        private readonly float _duration;

        private float _counter;

        public Wait(BehaviourTreeProperties properties) : base(properties)
        {
            _duration = properties.WaitDuration;
        }

        protected override void OnStop(BehaviourTreeContext context)
        {
            _counter = 0;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            _counter += deltaTime;

            var status = _counter >= _duration ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;

            return UniTask.FromResult(status);
        }
    }
}
