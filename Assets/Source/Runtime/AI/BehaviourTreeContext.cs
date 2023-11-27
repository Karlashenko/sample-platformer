using System.Collections.Generic;
using UnityEngine;

namespace Sample.AI
{
    public class BehaviourTreeContext
    {
        public readonly GameObject Owner;
        public readonly List<BehaviourTreeNode> RunningNodes = new();
        public readonly Dictionary<BehaviourTreeNode, int> RunningNodeIndices = new();

        public Vector3? TargetPoint;
        public GameObject? TargetUnit;
        public GameObject? PreviousTargetUnit;

        public BehaviourTreeContext(GameObject owner)
        {
            Owner = owner;
        }
    }
}