using System.Collections.Generic;

namespace Sample.AI
{
    public abstract class BehaviourTreeNodeContainer : BehaviourTreeNode
    {
        public readonly List<BehaviourTreeNode> Nodes = new();

        public void AddNode(BehaviourTreeNode node)
        {
            Nodes.Add(node);
        }
    }
}
