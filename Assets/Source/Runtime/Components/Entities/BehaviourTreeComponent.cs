using Sample.AI;
using UnityEngine;

namespace Sample.Components.Entities
{
    public class BehaviourTreeComponent : Component
    {
        public BehaviourTree BehaviourTree { get; private set; } = null!;

        [SerializeField] private BehaviourTreePresetType _preset;

        private void Start()
        {
            BehaviourTree = BehaviourTreePresets.Create(_preset, gameObject);
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
