using UnityEngine;

namespace Sample.Components
{
    public class FaceDirectionComponent : Component
    {
        [SerializeField] private MovementComponent _movement = null!;
        [SerializeField] private Transform _graphics = null!;

        private Vector2Int _previousDirection = new(1, 0);

        public void Update()
        {
            var direction = _movement.GetDirection();

            if (direction.x != _previousDirection.x)
            {
                _graphics.localRotation = Quaternion.Euler(0, direction.x > 0 ? 90 : -90, 0);
            }

            _previousDirection = direction;
        }
    }
}
