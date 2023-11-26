using System;

namespace Sample.AI
{
    [Serializable]
    public struct BehaviourTreeProperties
    {
        public float WaitDuration;
        public float FollowTargetStopDistance;
        public float Range;
    }
}