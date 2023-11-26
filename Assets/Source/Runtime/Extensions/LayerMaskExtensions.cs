using UnityEngine;

namespace Sample.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & 1 << layer) > 0;
        }
    }
}