using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sample.AI
{
    public abstract class BehaviourTreeNode
    {
        protected static readonly Collider2D[] ColliderCache = new Collider2D[32];

        private BehaviourTreeNode[] _interrupters = Array.Empty<BehaviourTreeNode>();

        private BehaviourTreeStatus _status;

        public BehaviourTreeStatus GetLastStatus()
        {
            return _status;
        }

        public async UniTask<BehaviourTreeStatus> Tick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            if (!context.RunningNodes.Contains(this))
            {
                if (!CanStart(context))
                {
                    return BehaviourTreeStatus.Failure;
                }

                OnStart(context);
                context.RunningNodes.Add(this);
            }

            if (await CheckConditions(deltaTime, context, cancellationToken))
            {
                _status = await OnTick(deltaTime, context, cancellationToken);
            }
            else
            {
                _status = BehaviourTreeStatus.Failure;
            }

            if (_status != BehaviourTreeStatus.Running)
            {
                OnStop(context);
                context.RunningNodes.Remove(this);
            }

            return _status;
        }

        public BehaviourTreeNode While(params BehaviourTreeNode[] interrupters)
        {
            _interrupters = interrupters;
            return this;
        }

        public virtual void DrawGizmos(BehaviourTreeContext context)
        {
        }

        protected abstract UniTask<BehaviourTreeStatus> OnTick(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken);

        protected virtual bool CanStart(BehaviourTreeContext context)
        {
            return true;
        }

        protected virtual void OnStart(BehaviourTreeContext context)
        {
        }

        protected virtual void OnStop(BehaviourTreeContext context)
        {
        }

        private async UniTask<bool> CheckConditions(float deltaTime, BehaviourTreeContext context, CancellationToken cancellationToken)
        {
            foreach (var interrupter in _interrupters)
            {
                var status = await interrupter.Tick(deltaTime, context, cancellationToken);

                if (status == BehaviourTreeStatus.Failure)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
