using UnityEngine;

namespace Sample.AI.Tasks
{
    public class MoveToTargetUnit : MoveToTarget
    {
        public MoveToTargetUnit(float stopDistance) : base(stopDistance)
        {
        }

        protected override Vector3 GetTargetPoint(BehaviourTreeContext context)
        {
            return context.TargetUnit!.transform.position;
        }
    }
}
