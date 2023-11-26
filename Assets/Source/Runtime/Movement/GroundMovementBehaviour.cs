using Sample.Extensions;
using Sample.Systems;
using Sample.Utils;
using UnityEngine;

namespace Sample.Movement
{
    public class GroundMovementBehaviour : MovementBehaviour
    {
        private const float JumpInputTolerance = 0.1f;
        private const float JumpLedgeTolerance = 0.2f;

        private readonly GroundMovementBehaviourSettings _settings;
        private readonly Configuration _configuration;
        private readonly CoroutineRunner _coroutineRunner;

        private float _jumpInputToleranceTimer;
        private float _jumpLedgeToleranceTimer;
        private float _velocityXSmoothing;
        private int _airJumpsRemaining;
        private int _horizontalInputDirection;
        private int _wallDirection;
        private bool _isWallSliding;
        private bool _isCrouching;
        private Vector3? _previousPlatformPosition;
        private Collider2D _previousPlatform;

        public GroundMovementBehaviour(Transform transform, BoxCollider2D collider, MovementSystem movementSystem, GroundMovementBehaviourSettings settings)
            : base(transform, collider, movementSystem, settings.ImpulseDamping)
        {
            _settings = settings;
            _configuration = Context.Get<Configuration>();
            _coroutineRunner = Context.Get<CoroutineRunner>();
        }

        public float GetGravity()
        {
            return _settings.Gravity;
        }

        public bool IsWallSliding()
        {
            return _isWallSliding;
        }

        protected override void OnUpdate(float deltaTime)
        {
            DetermineHorizontalInputDirection();

            HandleCrouch();
            HandleWallSliding();
            HandleFallThroughPlatforms();
            HandleMovingPlatforms();

            CalculateHorizontalVelocity();
            CalculateVerticalVelocity(deltaTime);

            ApplyJumpVelocity(deltaTime);
        }

        private void HandleMovingPlatforms()
        {
            if (MovementSystem.PlatformStandingOn == null)
            {
                _previousPlatformPosition = null;
                return;
            }

            if (MovementSystem.PlatformStandingOn != _previousPlatform)
            {
                _previousPlatformPosition = MovementSystem.PlatformStandingOn.transform.position;
            }

            var currentPlatformPosition = MovementSystem.PlatformStandingOn.transform.position;

            if (_previousPlatformPosition.HasValue)
            {
                Transform.Translate(currentPlatformPosition - _previousPlatformPosition.Value);
            }

            _previousPlatformPosition = currentPlatformPosition;
            _previousPlatform = MovementSystem.PlatformStandingOn;
        }

        private void HandleFallThroughPlatforms()
        {
            if (!Input.MoveDown || _isWallSliding || MovementSystem.IsFallingThroughPlatforms || !MovementSystem.CollisionBottom)
            {
                return;
            }

            Velocity.y = -2;

            // TODO: Replace time controller
            MovementSystem.IsFallingThroughPlatforms = true;
            _coroutineRunner.Wait(0.25f, () => MovementSystem.IsFallingThroughPlatforms = false);
        }

        private void HandleWallSliding()
        {
            var collidesWithTheWall = Mathf.Approximately(Mathf.Abs(MovementSystem.CollisionNormalHorizontal.x), 1);

            if (!_isWallSliding)
            {
                _wallDirection = MovementSystem.CollisionRight ? 1 : -1;
                _isWallSliding = collidesWithTheWall && _horizontalInputDirection == _wallDirection;
            }

            _isWallSliding = _isWallSliding && collidesWithTheWall && !MovementSystem.CollisionBottom;

            if (!_isWallSliding)
            {
                return;
            }

            Velocity.y = Mathf.Max(Velocity.y, -_settings.WallSlideSpeed);
        }

        private void HandleCrouch()
        {
            if (Input.Crouch)
            {
                if (_isCrouching)
                {
                    return;
                }

                _isCrouching = true;
                Collider.size *= new Vector2(1, 0.5f);
                Transform.position -= Vector3.up * 0.25f;
                return;
            }

            if (!_isCrouching || HasTopCollision(0.5f))
            {
                return;
            }

            _isCrouching = false;
            Collider.size *= new Vector2(1, 2);
            Transform.position += Vector3.up * 0.25f;
        }

        private void CalculateHorizontalVelocity()
        {
            if (_settings.DisableAirControl && Mathf.Abs(Velocity.y) > 0)
            {
                return;
            }

            var targetVelocityX = _isWallSliding ? 0 : _horizontalInputDirection * GetHorizontalVelocity();
            var smoothVelocityX = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref _velocityXSmoothing,
                MovementSystem.CollisionBottom ? _settings.AccelerationTimeGrounded : _settings.AccelerationTimeAirborne);

            Velocity.x = smoothVelocityX;
        }

        private void CalculateVerticalVelocity(float deltaTime)
        {
            if (MovementSystem.CollisionBottom && Velocity.y < 0)
            {
                Velocity.y = 0;
            }
            else if (MovementSystem.CollisionTop)
            {
                Velocity.y = -0.05f;
            }
            else
            {
                Velocity.y -= _settings.Gravity * deltaTime;
            }
        }

        private void DetermineHorizontalInputDirection()
        {
            _horizontalInputDirection = 0;

            if (Input.MoveLeft)
            {
                _horizontalInputDirection -= 1;
            }

            if (Input.MoveRight)
            {
                _horizontalInputDirection += 1;
            }
        }

        private float GetHorizontalVelocity()
        {
            return _isCrouching ? _settings.CrouchSpeed : _settings.MoveSpeed;
        }

        private void ApplyJumpVelocity(float deltaTime)
        {
            _jumpInputToleranceTimer = Mathf.Max(0, _jumpInputToleranceTimer - deltaTime);

            if (Input.JumpPressed)
            {
                if (!MovementSystem.CollisionBottom && !_isWallSliding && _airJumpsRemaining > 0)
                {
                    _airJumpsRemaining -= 1;
                    Velocity.y = _settings.JumpForce * (1 - _settings.AirJumpPenalty);
                    return;
                }

                _jumpInputToleranceTimer = JumpInputTolerance;
            }

            if (Input.JumpReleased && Velocity.y > 0 && !_isWallSliding)
            {
                Velocity.y *= 0.33f;
            }

            _jumpLedgeToleranceTimer = Mathf.Max(0, _jumpLedgeToleranceTimer - deltaTime);

            if (MovementSystem.CollisionBottom || _isWallSliding)
            {
                _jumpLedgeToleranceTimer = JumpLedgeTolerance;
                _airJumpsRemaining = _settings.AirJumpCount;
            }

            if (Mathf.Approximately(0, _jumpInputToleranceTimer) ||
                Mathf.Approximately(0, _jumpLedgeToleranceTimer))
            {
                return;
            }

            if (_isWallSliding)
            {
                var movingTowardsWall = _horizontalInputDirection == _wallDirection;
                var force = movingTowardsWall ? _settings.WallJumpForceClimb : _settings.WallJumpForceLeap;
                Velocity = new Vector2(force.x * -_wallDirection, force.y);
            }
            else
            {
                Velocity.y = _settings.JumpForce;
            }

            _jumpLedgeToleranceTimer = 0;
            _jumpInputToleranceTimer = 0;
        }

        public bool HasTopCollision(float length, int rayCount = 3)
        {
            var bounds = Collider.bounds.Expanded(-0.04f);
            var spacing = bounds.size.x / (rayCount - 1);

            return PhysicsUtils.RaycastMultiple(bounds.GetTopLeft(), Vector2.up, Vector2.right, rayCount, spacing, length, _configuration.CollisionMask);
        }
    }
}
