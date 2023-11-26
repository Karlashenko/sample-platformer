using UnityEngine;

namespace Sample
{
    public class Configuration : MonoBehaviour
    {
        public LayerMask CollisionMask;
        public LayerMask PlatformMask;
        public LayerMask UnitMask;
        public ContactFilter2D DamageContactFilter;
    }
}
