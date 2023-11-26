using UnityEngine;

namespace Sample.Movement
{
    public class GroundMovementBehaviourSettings : MonoBehaviour
    {
        [HideInInspector] public float Gravity;
        [HideInInspector] public float JumpForce;

        public float JumpHeight;
        public float TimeToJumpApex;
        public float MoveSpeed;
        public float CrouchSpeed;
        public float WallSlideSpeed;
        public float AccelerationTimeAirborne;
        public float AccelerationTimeGrounded;
        public float ImpulseDamping;
        public Vector2 DashForce;
        public Vector2 WallJumpForceLeap;
        public Vector2 WallJumpForceClimb;
        public bool DisableAirControl;
        public int AirJumpCount;

        [Range(0, 1)]
        public float AirJumpPenalty;

        private void OnValidate()
        {
            Prepare();
        }

        public void Prepare()
        {
            Gravity = 2 * JumpHeight / (TimeToJumpApex * TimeToJumpApex);
            JumpForce = Gravity * TimeToJumpApex;
        }
    }
}
