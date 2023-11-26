using UnityEngine;

namespace Sample.Utils
{
    public static class DebugUtils
    {
        public static void DrawRect(Vector3 center, Vector2 size, Color color, float duration = 1)
        {
            var halfSize = size / 2f;

            var bl = new Vector3(center.x - halfSize.x, center.y - halfSize.y);
            var br = new Vector3(center.x + halfSize.x, center.y - halfSize.y);
            var tr = new Vector3(center.x + halfSize.x, center.y + halfSize.y);
            var tl = new Vector3(center.x - halfSize.x, center.y + halfSize.y);

            Debug.DrawLine(bl, br, color, duration);
            Debug.DrawLine(br, tr, color, duration);
            Debug.DrawLine(tr, tl, color, duration);
            Debug.DrawLine(tl, bl, color, duration);
        }
    }
}
