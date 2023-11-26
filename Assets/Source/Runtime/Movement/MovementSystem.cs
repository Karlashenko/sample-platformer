using Sample.Extensions;
using Sample.Utils;
using UnityEngine;

namespace Sample.Movement
{
    public class MovementSystem
    {
        private const int HorizontalRayCount = 4;
        private const int VerticalRayCount = 3;
        private const float MaxSlopeAngle = 70;
        private const float BoundingBoxInset = 0.2f;

        public bool CollisionTop;
        public bool CollisionBottom;
        public bool CollisionLeft;
        public bool CollisionRight;
        public bool IsStandingOnPlatform;
        public float SlopeAngle;
        public bool MovingUpTheSlope;
        public bool MovingDownTheSlope;
        public bool SlidingDownTheSlope;
        public Collider2D PlatformStandingOn;
        public Vector2 CollisionNormalHorizontal;
        public Vector2 CollisionNormalVertical;
        public bool IsCollisionEnabled = true;
        public bool IsFallingThroughPlatforms;
        public Vector2Int RaycastDirection;

        private readonly float _maxSlopeAngleTangent = Mathf.Tan(MaxSlopeAngle * Mathf.Deg2Rad);
        private readonly Configuration _configuration;

        public MovementSystem()
        {
            _configuration = Context.Get<Configuration>();
        }

        public void Translate(Transform transform, BoxCollider2D collider, Vector2 translation)
        {
            RaycastDirection.x = Mathf.Approximately(translation.x, 0) ? RaycastDirection.x : translation.x.CompareTo(0);
            RaycastDirection.y = Mathf.Approximately(translation.y, 0) ? RaycastDirection.y : translation.y.CompareTo(0);

            ResetState();

            if (IsCollisionEnabled)
            {
                var bounds = collider.bounds.Shrinked(BoundingBoxInset * 2);

                #if UNITY_EDITOR
                Debug.DrawLine(bounds.GetBottomLeft(), bounds.GetBottomRight(), Color.cyan);
                Debug.DrawLine(bounds.GetBottomRight(), bounds.GetTopRight(), Color.cyan);
                Debug.DrawLine(bounds.GetTopRight(), bounds.GetTopLeft(), Color.cyan);
                Debug.DrawLine(bounds.GetTopLeft(), bounds.GetBottomLeft(), Color.cyan);
                #endif

                //TranslateUpTheSlope(ref translation, bounds);

                if (!MovingUpTheSlope)
                {
                    CollideHorizontally(ref translation, bounds, transform);
                }

                //TranslateDownTheSlope(ref translation, bounds);

                if (!MovingDownTheSlope && !SlidingDownTheSlope)
                {
                    CollideVertically(ref translation, bounds, transform);
                }
            }

            transform.Translate(translation);
        }

        private void ResetState()
        {
            CollisionTop = false;
            CollisionBottom = false;
            CollisionLeft = false;
            CollisionRight = false;
            MovingUpTheSlope = false;
            MovingDownTheSlope = false;
            SlidingDownTheSlope = false;
            IsStandingOnPlatform = false;
            PlatformStandingOn = null;
            SlopeAngle = 0;
            CollisionNormalVertical = new Vector2();
            CollisionNormalHorizontal = new Vector2();
        }

        private void CollideHorizontally(ref Vector2 translation, in Bounds bounds, Transform transform)
        {
            var raySpacing = bounds.size.y / (HorizontalRayCount - 1);
            var rayLength = Mathf.Max(BoundingBoxInset + Mathf.Abs(translation.x), BoundingBoxInset + 0.1f);
            var rayOrigin = RaycastDirection.x > 0 ? bounds.GetBottomRight() : bounds.GetBottomLeft();
            var rayDirection = Vector2.right * RaycastDirection.x;

            var raycastHit = PhysicsUtils.RaycastMultiple(rayOrigin, rayDirection, Vector2.up, HorizontalRayCount, raySpacing, rayLength, _configuration.CollisionMask);

            if (!raycastHit)
            {
                return;
            }

            CollisionNormalHorizontal = raycastHit.normal;
            CollisionRight = RaycastDirection.x > 0;
            CollisionLeft = RaycastDirection.x < 0;

            Debug.DrawLine(raycastHit.point, rayOrigin, Color.red, 10);

            var position = transform.position;
            position = position.With(x: raycastHit.point.x - (bounds.size.x / 2f + BoundingBoxInset) * RaycastDirection.x);
            transform.position = position;
            translation.x = 0;
        }

        private void CollideVertically(ref Vector2 translation, in Bounds bounds, Transform transform)
        {
            var raySpacing = bounds.size.x / (VerticalRayCount - 1);
            var rayLength = Mathf.Max(BoundingBoxInset + Mathf.Abs(translation.y), BoundingBoxInset + 0.1f);
            var rayOrigin = RaycastDirection.y > 0 ? bounds.GetTopLeft() : bounds.GetBottomLeft();
            var rayDirection = Vector2.up * RaycastDirection.y;

            var raycastHit = PhysicsUtils.RaycastMultiple(rayOrigin, rayDirection, Vector2.right, VerticalRayCount, raySpacing, rayLength, _configuration.CollisionMask | _configuration.PlatformMask);

            if (!raycastHit)
            {
                return;
            }

            CollisionNormalVertical = raycastHit.normal;

            var hitsPlatform = _configuration.PlatformMask.ContainsLayer(raycastHit.collider.gameObject.layer);

            if (hitsPlatform)
            {
                if (translation.y < 0)
                {
                    var slopeDirection = raycastHit.normal.x.CompareTo(0);
                    var slopeContactPointX = slopeDirection > 0 ? bounds.min.x : bounds.max.x;

                    if (!Mathf.Approximately(slopeDirection, 0) &&
                        !Mathf.Approximately(slopeContactPointX, raycastHit.point.x))
                    {
                        return;
                    }
                }

                if (rayDirection.y > 0 || IsFallingThroughPlatforms)
                {
                    return;
                }
            }

            CollisionTop = RaycastDirection.y > 0;
            CollisionBottom = RaycastDirection.y < 0;
            IsStandingOnPlatform = hitsPlatform && CollisionBottom;
            PlatformStandingOn = IsStandingOnPlatform ? raycastHit.collider : null;

            var position = transform.position;
            position = position.With(y: raycastHit.point.y - (bounds.size.y / 2f + BoundingBoxInset) * RaycastDirection.y);
            transform.position = position;
            translation.y = 0;
        }

        private void TranslateUpTheSlope(ref Vector2 translation, in Bounds bounds)
        {
            if (IsFallingThroughPlatforms || Mathf.Approximately(translation.x, 0))
            {
                return;
            }

            var rayLength = BoundingBoxInset + Mathf.Abs(translation.x);
            var rayOrigin = RaycastDirection.x > 0 ? bounds.GetBottomRight() : bounds.GetBottomLeft();

            var raycastHit = Physics2D.Raycast(rayOrigin, Vector2.right * RaycastDirection.x, rayLength, _configuration.CollisionMask | _configuration.PlatformMask);

            if (!raycastHit)
            {
                return;
            }

            var slopeAngle = Vector2.Angle(raycastHit.normal, Vector2.up);

            if (slopeAngle > MaxSlopeAngle)
            {
                return;
            }

            var step = Mathf.Abs(translation.x);
            var translationY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * step;

            // Jumping
            if (translation.y > translationY)
            {
                return;
            }

            MovingUpTheSlope = true;
            SlopeAngle = slopeAngle;
            CollisionBottom = true;
            PlatformStandingOn = raycastHit.collider;
            IsStandingOnPlatform = _configuration.PlatformMask.ContainsLayer(raycastHit.collider.gameObject.layer);

            translation.x = Mathf.Cos(SlopeAngle * Mathf.Deg2Rad) * step * RaycastDirection.x;
            translation.y = translationY;
        }

        private void TranslateDownTheSlope(ref Vector2 translation, in Bounds bounds)
        {
            if (SlideDownTheSlope(ref translation, bounds.GetBottomLeft()) ||
                SlideDownTheSlope(ref translation, bounds.GetBottomRight()))
            {
                return;
            }

            if (IsFallingThroughPlatforms || Mathf.Approximately(translation.x, 0) || translation.y > 0)
            {
                return;
            }

            var raycastOrigin = RaycastDirection.x > 0 ? bounds.GetBottomLeft() : bounds.GetBottomRight();
            var raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, Mathf.Infinity, _configuration.CollisionMask | _configuration.PlatformMask);

            if (!raycastHit || !Mathf.Approximately(Mathf.Sign(raycastHit.normal.x), RaycastDirection.x))
            {
                return;
            }

            if (raycastHit.distance - BoundingBoxInset > _maxSlopeAngleTangent * Mathf.Abs(translation.x))
            {
                return;
            }

            var slopeAngle = Vector2.Angle(Vector2.up, raycastHit.normal);

            if (Mathf.Approximately(slopeAngle, 0))
            {
                return;
            }

            MovingDownTheSlope = true;
            SlopeAngle = slopeAngle;
            CollisionBottom = true;
            PlatformStandingOn = raycastHit.collider;
            IsStandingOnPlatform = _configuration.PlatformMask.ContainsLayer(raycastHit.collider.gameObject.layer);

            translation.y -= Mathf.Abs(raycastHit.point.y - raycastOrigin.y) - BoundingBoxInset;
        }

        private bool SlideDownTheSlope(ref Vector2 translation, in Vector2 origin)
        {
            var raycastHit = Physics2D.Raycast(origin, Vector3.down, Mathf.Abs(translation.y) + BoundingBoxInset, _configuration.CollisionMask | _configuration.PlatformMask);

            if (!raycastHit)
            {
                return false;
            }

            var slopeAngle = Vector2.Angle(raycastHit.normal, Vector2.up);

            if (slopeAngle < MaxSlopeAngle)
            {
                return false;
            }

            SlopeAngle = slopeAngle;
            SlidingDownTheSlope = true;

            translation.x = (Mathf.Abs(translation.y) - Mathf.Abs(raycastHit.point.x - origin.x)) / Mathf.Tan(SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(raycastHit.normal.x);

            return true;
        }
    }
}
