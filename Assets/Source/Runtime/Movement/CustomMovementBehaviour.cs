using UnityEngine;

namespace Sample.Movement
{
    public class CustomMovementBehaviour : MovementBehaviour
    {
        public CustomMovementBehaviour(Transform transform, BoxCollider2D collider, MovementSystem movementSystem)
            : base(transform, collider, movementSystem, 0)
        {
        }

        protected override void OnEnable()
        {
            MovementSystem.IsCollisionEnabled = false;
        }

        protected override void OnDisable()
        {
            MovementSystem.IsCollisionEnabled = true;
        }
    }
}