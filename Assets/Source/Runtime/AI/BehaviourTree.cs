using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sample.AI
{
    public class BehaviourTree
    {
        public readonly BehaviourTreeContext Context;
        public readonly BehaviourTreeNodeContainer Root;

        public BehaviourTree(BehaviourTreeNodeContainer root, GameObject unit)
        {
            Root = root;
            Context = new BehaviourTreeContext(unit);
        }

        public BehaviourTreeNode GetFirstRunningNode()
        {
            return Context.RunningNodes.Count > 0 ? Context.RunningNodes.First() : Root;
        }

        public async UniTask Tick(float deltaTime, CancellationToken cancellationToken)
        {
            await GetFirstRunningNode().Tick(deltaTime, Context, cancellationToken);
        }

        public void DrawGizmos()
        {
            GetFirstRunningNode().DrawGizmos();
        }
    }
}
