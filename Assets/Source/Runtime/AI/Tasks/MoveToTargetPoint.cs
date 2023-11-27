using UnityEngine;

namespace Sample.AI.Tasks
{
    public class MoveToTargetPoint : MoveToTarget
    {
        public MoveToTargetPoint(float stopDistance) : base(stopDistance)
        {
        }

        protected override Vector3 GetTargetPoint(BehaviourTreeContext context)
        {
            return context.TargetPoint!.Value;
        }
    }
}
