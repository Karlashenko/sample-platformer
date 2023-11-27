using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FlyToTargetPoint : FlyToTarget
    {
        public FlyToTargetPoint(float stopDistance) : base(stopDistance)
        {
        }

        protected override Vector3 GetTargetPoint(BehaviourTreeContext context)
        {
            return context.TargetPoint!.Value;
        }
    }
}
