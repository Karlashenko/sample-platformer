using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sample.AI
{
    public class BehaviourTreeBuilder
    {
        private readonly Stack<BehaviourTreeCompositeNode> _containers = new();

        private BehaviourTreeCompositeNode _top;

        public BehaviourTreeBuilder Begin(BehaviourTreeCompositeNode container)
        {
            if (_containers.Count > 0)
            {
                _containers.Peek().AddNode(container);
            }

            _containers.Push(container);

            return this;
        }

        public BehaviourTreeBuilder Node(BehaviourTreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_containers.Count <= 0)
            {
                throw new Exception("There is no parent node in the tree.");
            }

            _containers.Peek().AddNode(node);

            return this;
        }

        public BehaviourTreeBuilder End()
        {
            _top = _containers.Pop();

            return this;
        }

        public BehaviourTree Build(GameObject unit)
        {
            if (_top == null)
            {
                throw new Exception("Can't create a behaviour tree with zero nodes");
            }

            return new BehaviourTree(_top, unit);
        }
    }
}