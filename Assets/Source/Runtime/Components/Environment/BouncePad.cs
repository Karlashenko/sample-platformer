using Sample.Components.Entities;
using Sample.Extensions;
using UnityEngine;

namespace Sample.Components.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BouncePad : MonoBehaviour
    {
        [SerializeField] private float _force;

        private BoxCollider2D _collider = null!;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var movementComponent = other.GetComponent<MovementComponent>();

            if (movementComponent == null)
            {
                return;
            }

            var origin = Mathf.Abs(transform.localRotation.z) > 0 ?
                _collider.GetTransformedTopCenter() : _collider.GetTransformedBottomCenter();

            if (Vector2.Dot(transform.up, (other.bounds.GetBottomCenter() - origin).normalized) < 0)
            {
                return;
            }

            var velocity = transform.up * _force;

            movementComponent.SetVelocity(velocity.With(movementComponent.GetVelocity().x + velocity.x));
        }
    }
}
