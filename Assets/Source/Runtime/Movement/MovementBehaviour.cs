using Sample.Components.Entities;
using Sample.Extensions;
using UnityEngine;

namespace Sample.Movement
{
    public abstract class MovementBehaviour
    {
        public Vector2 Velocity;

        protected InputState Input;

        protected readonly Transform Transform;
        protected readonly BoxCollider2D Collider;
        protected readonly MovementSystem MovementSystem;

        private Vector2 _impulse;
        private Vector2 _impulseBase;
        private readonly float _impulseDamping;

        protected MovementBehaviour(Transform transform, BoxCollider2D collider, MovementSystem movementSystem, float impulseDamping)
        {
            Transform = transform;
            Collider = collider;
            MovementSystem = movementSystem;
            _impulseDamping = impulseDamping;
        }

        public Vector2 GetImpulse()
        {
            return _impulse;
        }

        public void SetImpulse(float? x = null, float? y = null)
        {
            _impulse = _impulse.With(x, y);
            _impulseBase = _impulse;
        }

        public void AddImpulse(Vector2 impulse)
        {
            _impulse += impulse;
            _impulseBase = _impulse;
        }

        public void SetInput(in InputState input)
        {
            Input = input;
        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);

            MovementSystem.Translate(Transform, Collider, (Velocity + _impulse) * deltaTime);

            ApplyImpulseDrag(deltaTime);
        }

        public void Enable()
        {
            OnEnable();
        }

        public void Disable()
        {
            OnDisable();
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        private void ApplyImpulseDrag(float deltaTime)
        {
            // TODO:

            if (_impulse == Vector2.zero)
            {
                return;
            }

            _impulse *= 1 - deltaTime * _impulseDamping;

            if (_impulse.x.CompareTo(0) != Velocity.x.CompareTo(0))
            {
                _impulse.x -= Mathf.Abs(Velocity.x) * deltaTime * 15 * Mathf.Sign(_impulse.x);
            }

            if (_impulse.y.CompareTo(0) != Velocity.y.CompareTo(0))
            {
                _impulse.y -= Mathf.Abs(Velocity.y) * deltaTime * 15 * Mathf.Sign(_impulse.y);
            }

            if (_impulse.x.CompareTo(0) != _impulseBase.x.CompareTo(0))
            {
                _impulse.x = 0;
            }

            if (_impulse.y.CompareTo(0) != _impulseBase.y.CompareTo(0))
            {
                _impulse.y = 0;
            }

            if (MovementSystem.CollisionLeft || MovementSystem.CollisionRight)
            {
                _impulse.x = 0;
            }

            if (MovementSystem.CollisionTop || MovementSystem.CollisionBottom)
            {
                _impulse.y = 0;
            }
        }
    }
}
