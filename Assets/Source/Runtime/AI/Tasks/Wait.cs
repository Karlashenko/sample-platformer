using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class Wait : BehaviourTreeNode
    {
        private readonly float _min;
        private readonly float _max;

        private float _duration;

        private float _counter;

        public Wait(float min, float? max)
        {
            _min = min;
            _max = max ?? min;
        }

        protected override void OnStart(BehaviourTreeContext context)
        {
            _duration = Random.Range(_min, _max);
        }

        protected override void OnStop(BehaviourTreeContext context)
        {
            _counter = 0;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            _counter += deltaTime;

            var status = _counter >= _duration ?
                BehaviourTreeStatus.Success : BehaviourTreeStatus.Running;

            return UniTask.FromResult(status);
        }
    }
}
