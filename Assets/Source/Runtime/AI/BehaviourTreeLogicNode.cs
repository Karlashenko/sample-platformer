using UnityEngine;

namespace Sample.AI
{
    public abstract class BehaviourTreeLogicNode : BehaviourTreeNode
    {
        protected static readonly Collider2D[] ColliderCache = new Collider2D[32];

        protected readonly BehaviourTreeProperties Properties;

        protected BehaviourTreeLogicNode(BehaviourTreeProperties properties)
        {
            Properties = properties;
        }
    }
}