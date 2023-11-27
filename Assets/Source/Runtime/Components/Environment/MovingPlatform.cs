using UnityEngine;

namespace Sample.Components.Environment
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private float _velocity;
        [SerializeField] private Transform[] _checkpoints = null!;

        private int _checkpointIndex;
        private int _checkpointDirection = 1;

        private void Update()
        {
            var distance = (transform.position - _checkpoints[_checkpointIndex].position).magnitude;

            if (distance < _velocity * Time.deltaTime * 2)
            {
                _checkpointIndex += _checkpointDirection;

                if (_checkpointIndex == -1 || _checkpointIndex == _checkpoints.Length)
                {
                    _checkpointIndex = Mathf.Clamp(_checkpointIndex, 0, _checkpoints.Length - 1);
                    _checkpointDirection *= -1;
                }

                return;
            }

            var direction = (_checkpoints[_checkpointIndex].position - transform.position).normalized;
            var translation = direction * _velocity * Time.deltaTime;

            transform.Translate(translation);
        }
    }
}
