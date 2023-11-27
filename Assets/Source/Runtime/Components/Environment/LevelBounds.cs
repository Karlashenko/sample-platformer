using Sample.Components.Entities;
using Sample.Values;
using UnityEngine;

namespace Sample.Components.Environment
{
    public class LevelBounds : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            var health = other.GetComponent<HealthComponent>();

            if (health)
            {
                health.Damage(new Damage(int.MaxValue, DamageType.True), health.gameObject);
            }
        }
    }
}
