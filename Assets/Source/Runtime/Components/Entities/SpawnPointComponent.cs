using UnityEngine;

namespace Sample.Components.Entities
{
    public class SpawnPointComponent : MonoBehaviour
    {
        public Vector3 SpawnPoint { get; private set; }

        public void Construct(Vector3 spawnPoint)
        {
            SpawnPoint = spawnPoint;
        }
    }
}
