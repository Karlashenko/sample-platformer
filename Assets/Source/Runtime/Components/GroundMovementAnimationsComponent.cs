using UnityEngine;

namespace Sample.Components
{
    public class GroundMovementAnimationsComponent : Component
    {
        private static readonly int _animatorParameterRunning = Animator.StringToHash("Running");
        private static readonly int _animatorParameterGrounded = Animator.StringToHash("Grounded");
        private static readonly int _animatorParameterWallSliding = Animator.StringToHash("WallSliding");
        private static readonly int _animatorParameterVelocityY = Animator.StringToHash("Velocity_Y");

        [SerializeField] private MovementComponent _movement = null!;
        [SerializeField] private Animator _animator = null!;

        private void Update()
        {
            var velocity = _movement.GetVelocity();

            _animator.SetFloat(_animatorParameterVelocityY, velocity.y);
            _animator.SetBool(_animatorParameterRunning, Mathf.Abs(velocity.x) > 0 && _movement.IsGrounded());
            _animator.SetBool(_animatorParameterGrounded, _movement.IsGrounded());
            _animator.SetBool(_animatorParameterWallSliding, _movement.IsWallSliding());
        }
    }
}
