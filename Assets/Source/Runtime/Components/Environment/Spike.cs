using Sample.Components.Entities;
using Sample.Values;
using UnityEngine;

namespace Sample.Components.Environment
{
    public class Spike : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerComponent>())
            {
                return;
            }

            other.GetComponent<HealthComponent>().Damage(new Damage(1, DamageType.Piercing), gameObject);
        }
    }
}
