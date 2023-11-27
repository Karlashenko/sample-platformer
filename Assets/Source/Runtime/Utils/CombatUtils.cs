using Sample.Components.Entities;
using UnityEngine;

namespace Sample.Utils
{
    public static class CombatUtils
    {
        public static bool IsEnemyOf(this GameObject self, GameObject other)
        {
            if (self.GetComponent<MonsterComponent>())
            {
                return other.GetComponent<PlayerComponent>();
            }

            if (self.GetComponent<PlayerComponent>())
            {
                return other.GetComponent<MonsterComponent>();
            }

            return false;
        }
    }
}
