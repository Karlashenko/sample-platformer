using UnityEngine;

namespace Sample.AI.Tasks
{
    public class FlyToTargetUnit : FlyToTarget
    {
        public FlyToTargetUnit(float stopDistance) : base(stopDistance)
        {
        }

        protected override Vector3 GetTargetPoint(BehaviourTreeContext context)
        {
            return context.TargetUnit!.transform.position;
        }
    }
}
