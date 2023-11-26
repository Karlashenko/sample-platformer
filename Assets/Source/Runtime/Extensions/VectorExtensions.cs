using System;
using UnityEngine;

namespace Sample.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 Round(this Vector3 vector, int precision)
        {
            return new Vector3(
                (float) Math.Round(vector.x, precision),
                (float) Math.Round(vector.y, precision),
                (float) Math.Round(vector.z, precision)
            );
        }

        public static Vector2 Round(this Vector2 vector, int precision)
        {
            return new Vector2(
                (float) Math.Round(vector.x, precision),
                (float) Math.Round(vector.y, precision)
            );
        }

        public static Vector3 Snap(this Vector3 vector, float snap)
        {
            var xCount = Mathf.RoundToInt(vector.x / snap);
            var yCount = Mathf.RoundToInt(vector.y / snap);

            return new Vector3(xCount * snap, yCount * snap, 0);
        }

        public static Vector2 To2DAxis(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }

        public static Vector2 Snap(this Vector2 vector, float cellSize)
        {
            var xCount = Mathf.RoundToInt(vector.x / cellSize);
            var yCount = Mathf.RoundToInt(vector.y / cellSize);

            return new Vector2(xCount * cellSize, yCount * cellSize);
        }

        public static float DirectionToRadians(this Vector3 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        public static float DirectionToRadians(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        public static float DirectionToDegrees(this Vector3 vector)
        {
            return vector.DirectionToRadians() * Mathf.Rad2Deg;
        }

        public static float DirectionToDegrees(this Vector2 vector)
        {
            return vector.DirectionToRadians() * Mathf.Rad2Deg;
        }

        public static float DirectionToDegrees360(this Vector3 vector)
        {
            var angle = vector.DirectionToDegrees();

            if (angle < 0)
            {
                angle += 360;
            }

            if (angle > 360)
            {
                angle -= 360;
            }

            return angle;
        }

        public static Vector3 DegreesToDirection(this float angle)
        {
            var radians = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        }
    }
}