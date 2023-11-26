using UnityEngine;

namespace Sample.Extensions
{
    public static class BoundsExtensions
    {
        public static Bounds Expanded(this Bounds bounds, float amount)
        {
            var result = bounds;
            result.Expand(amount);

            return result;
        }

        public static Bounds Shrinked(this Bounds bounds, float amount)
        {
            return bounds.Expanded(-amount);
        }

        public static Bounds WithSize(this Bounds bounds, Vector2 size)
        {
            var result = bounds;
            result.size = size;

            return result;
        }

        public static Bounds WithCenter(this Bounds bounds, Vector2 center)
        {
            var result = bounds;
            result.center = center;

            return result;
        }

        public static Vector3 GetTopCenter(this Bounds bounds)
        {
            return new Vector3(bounds.center.x, bounds.max.y);
        }

        public static Vector3 GetBottomCenter(this Bounds bounds)
        {
            return new Vector3(bounds.center.x, bounds.min.y);
        }

        public static Vector3 GetBottomLeft(this Bounds bounds)
        {
            return bounds.min;
        }

        public static Vector3 GetBottomRight(this Bounds bounds)
        {
            return new Vector3(bounds.max.x, bounds.min.y);
        }

        public static Vector3 GetTopLeft(this Bounds bounds)
        {
            return new Vector3(bounds.min.x, bounds.max.y);
        }

        public static Vector3 GetTopRight(this Bounds bounds)
        {
            return bounds.max;
        }
    }
}