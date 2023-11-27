using UnityEngine;

namespace Sample.Systems
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Camera _camera = null!;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothFactor;

        private Transform? _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void UnsetTarget()
        {
            _target = null;
        }

        private void Update()
        {
            if (_target is null)
            {
                return;
            }

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position + _offset, _smoothFactor * Time.deltaTime);
        }
    }
}
