using UnityEngine;

namespace Sample.Systems
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothFactor;

        private void Update()
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position + _offset, _smoothFactor * Time.deltaTime);
        }
    }
}
