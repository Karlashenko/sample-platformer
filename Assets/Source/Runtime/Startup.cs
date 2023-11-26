using Sample.Pathfinding;
using Sample.Systems;
using UnityEngine;

namespace Sample
{
    public class Startup : MonoBehaviour
    {
        private void Awake()
        {
            Context.Set(FindFirstObjectByType<Pathfinder>());
            Context.Set(FindFirstObjectByType<PathfinderTest>());
            Context.Set(FindFirstObjectByType<Configuration>());
            Context.Set(FindFirstObjectByType<CoroutineRunner>());
            Context.Set(FindFirstObjectByType<Waypoints>());
        }
    }
}
