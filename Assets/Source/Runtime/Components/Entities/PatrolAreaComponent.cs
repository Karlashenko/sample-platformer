using UnityEngine;

namespace Sample.Components.Entities
{
    public class PatrolAreaComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _range;

        public Vector3 GetNextPoint()
        {
            return transform.position + new Vector3(
                Random.Range(-_range.x, _range.x),
                Random.Range(-_range.y, _range.y), 0);
        }
    }
}
