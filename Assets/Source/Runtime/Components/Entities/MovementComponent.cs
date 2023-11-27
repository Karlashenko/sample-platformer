using Sample.Movement;
using UnityEngine;

namespace Sample.Components.Entities
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MovementComponent : Component
    {
        [SerializeField] private GroundMovementBehaviourSettings _groundMovementBehaviourSettings = null!;

        public MovementBehaviourType MovementBehaviourType;

        private GroundMovementBehaviour _groundMovementBehaviour = null!;
        private CustomMovementBehaviour _customMovementBehaviour = null!;
        private MovementBehaviour _currentMovementBehaviour = null!;
        private MovementSystem _movementSystem = null!;

        private void Start()
        {
            var boxCollider2D = GetComponent<BoxCollider2D>();

            _movementSystem = new MovementSystem();

            // TODO:

            if (MovementBehaviourType == MovementBehaviourType.Ground)
            {
                _groundMovementBehaviourSettings.Prepare();
                _groundMovementBehaviour = new GroundMovementBehaviour(transform, boxCollider2D, _movementSystem, _groundMovementBehaviourSettings!);
                SwitchToGroundMovementBehaviour();
            }
            else
            {
                _customMovementBehaviour = new CustomMovementBehaviour(transform, boxCollider2D, _movementSystem);
                SwitchToCustomMovementBehaviour();
            }
        }

        private void Update()
        {
            _currentMovementBehaviour.Update(Time.deltaTime);
        }

        public Vector2Int GetDirection()
        {
            return _movementSystem.RaycastDirection;
        }

        public bool IsGrounded()
        {
            return _movementSystem.CollisionBottom;
        }

        public bool IsWallSliding()
        {
            return _groundMovementBehaviour?.IsWallSliding() == true;
        }

        public bool IsAtTheWall()
        {
            return _movementSystem.CollisionLeft || _movementSystem.CollisionRight;
        }

        public void EnableCollision()
        {
            _movementSystem.IsCollisionEnabled = true;
        }

        public void DisableCollision()
        {
            _movementSystem.IsCollisionEnabled = false;
        }

        public float GetGravity()
        {
            return _groundMovementBehaviour!.GetGravity();
        }

        public Vector2 GetVelocity()
        {
            return _currentMovementBehaviour.Velocity;
        }

        public void AddImpulse(Vector2 impulse)
        {
            _currentMovementBehaviour.AddImpulse(impulse);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _currentMovementBehaviour.Velocity = velocity;
        }

        public void SetVelocityX(float velocityX)
        {
            _currentMovementBehaviour.Velocity.x = velocityX;
        }

        public void SetVelocityY(float velocityY)
        {
            _currentMovementBehaviour.Velocity.y = velocityY;
        }

        public void AddVelocity(Vector2 velocity)
        {
            _currentMovementBehaviour.Velocity += velocity;
        }

        public void SetInput(InputState input)
        {
            _currentMovementBehaviour?.SetInput(input);
        }

        public void SwitchToGroundMovementBehaviour()
        {
            SwitchMovementBehaviour(_groundMovementBehaviour);
        }

        public void SwitchToCustomMovementBehaviour()
        {
            SwitchMovementBehaviour(_customMovementBehaviour);
        }

        private void SwitchMovementBehaviour(MovementBehaviour movementBehaviour)
        {
            if (_currentMovementBehaviour == movementBehaviour)
            {
                return;
            }

            _currentMovementBehaviour?.Disable();
            _currentMovementBehaviour = movementBehaviour;
            _currentMovementBehaviour.Enable();
        }
    }
}
