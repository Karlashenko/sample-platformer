using Sample.AI;
using UnityEngine;

namespace Sample.Components
{
    public class BehaviourTreeComponent : Component
    {
        public BehaviourTree BehaviourTree { get; private set; } = null!;

        [SerializeField] private BehaviourTreePresetType _preset;
        [SerializeField] private BehaviourTreeProperties _properties;

        private void Start()
        {
            BehaviourTree = BehaviourTreePresets.Create(_preset, gameObject, _properties);
        }

        private async void Update()
        {
            await BehaviourTree.Tick(Time.deltaTime, destroyCancellationToken);
        }

        private void OnDrawGizmos()
        {
            BehaviourTree?.DrawGizmos();
        }
    }
}
