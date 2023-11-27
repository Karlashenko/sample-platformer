using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sample.AI.Tasks
{
    public class DebugLog : BehaviourTreeNode
    {
        private readonly string _message;

        public DebugLog(string message)
        {
            _message = message;
        }

        protected override UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            Debug.Log(_message);
            return UniTask.FromResult(BehaviourTreeStatus.Success);
        }
    }
}
