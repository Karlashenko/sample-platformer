using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.AI
{
    public abstract class BehaviourTreeNode
    {
        private BehaviourTreeStatus _status;

        public BehaviourTreeStatus GetLastStatus()
        {
            return _status;
        }

        public async UniTask<BehaviourTreeStatus> Tick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            if (!context.RunningNodes.Contains(this))
            {
                context.RunningNodes.Add(this);
                OnStart(context);
            }

            _status = await OnTick(deltaTime, context, cancellationToken);

            if (_status != BehaviourTreeStatus.Running)
            {
                context.RunningNodes.Remove(this);
                OnStop(context);
            }

            return _status;
        }

        public virtual void DrawGizmos()
        {
        }

        protected abstract UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken);

        protected virtual void OnStart(BehaviourTreeContext context)
        {
        }

        protected virtual void OnStop(BehaviourTreeContext context)
        {
        }
    }
}