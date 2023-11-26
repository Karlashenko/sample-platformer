using System;
using UnityEngine;

namespace Sample.Utils
{
    public static class PhysicsUtils
    {
        private static readonly RaycastHit2D[] _raycastCache = new RaycastHit2D[64];
        private static readonly Collider2D[] _colliderCache = new Collider2D[64];

        public static ReadOnlySpan<Collider2D> OverlapCircle(Vector2 origin, float radius, LayerMask mask)
        {
            var targets = Physics2D.OverlapCircleNonAlloc(origin, radius, _colliderCache, mask);

            return new ReadOnlySpan<Collider2D>(_colliderCache, 0, targets);
        }

        public static ReadOnlySpan<Collider2D> RaycastAll(Vector3 origin, Vector2 direction, float distance, LayerMask mask)
        {
            var targets = Physics2D.RaycastNonAlloc(origin, direction, _raycastCache, distance, mask);

            return new ReadOnlySpan<Collider2D>(_colliderCache, 0, targets);
        }

        public static RaycastHit2D RaycastMultiple(Vector2 origin, Vector2 direction, Vector2 offsetDirection, int count, float spacing, float distance, LayerMask mask)
        {
            for (var i = 0; i < count; i++)
            {
                var offset = origin + offsetDirection * (spacing * i);
                _raycastCache[i] = Physics2D.Raycast(offset, direction, distance, mask);

            #if UNITY_EDITOR
                var hitDistance = _raycastCache[i].collider ? _raycastCache[i].distance : distance;
                Debug.DrawRay(offset, direction * hitDistance, Color.cyan);
            #endif
            }

            var minDistance = 100f;
            var index = 0;

            for (var i = 0; i < count; i++)
            {
                if (!_raycastCache[i] || _raycastCache[i].distance >= minDistance)
                {
                    continue;
                }

                minDistance = _raycastCache[i].distance;
                index = i;
            }

            return _raycastCache[index];
        }
    }
}
