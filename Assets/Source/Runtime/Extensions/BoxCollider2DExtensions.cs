using UnityEngine;

namespace Sample.Extensions
{
    public static class BoxCollider2DExtensions
    {
        public static Vector3 GetTransformedTopCenter(this BoxCollider2D collider)
        {
            return collider.transform.TransformPoint(new Vector3(collider.size.x / 2, collider.size.y));
        }

        public static Vector3 GetTransformedBottomCenter(this BoxCollider2D collider)
        {
            return collider.transform.TransformPoint(new Vector3(collider.size.x / 2, 0));
        }
    }
}