using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sample.AI
{
    public class BehaviourTree
    {
        public readonly BehaviourTreeContext Context;
        public readonly BehaviourTreeCompositeNode Root;

        public BehaviourTree(BehaviourTreeCompositeNode root, GameObject unit)
        {
            Root = root;
            Context = new BehaviourTreeContext(unit);
        }

        public BehaviourTreeNode GetFirstRunningNode()
        {
            return Context.RunningNodes.Count > 0 ? Context.RunningNodes[0] : Root;
        }

        public async UniTask Tick(float deltaTime, CancellationToken cancellationToken)
        {
            await GetFirstRunningNode().Tick(deltaTime, Context, cancellationToken);
        }

        public void DrawGizmos()
        {
            GetFirstRunningNode().DrawGizmos(Context);
        }
    }
}
