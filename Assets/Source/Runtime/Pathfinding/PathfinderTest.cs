using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Sample.Pathfinding
{
    public class PathfinderTest : MonoBehaviour
    {
        [SerializeField] private Waypoints _waypoints;
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private Transform _origin;
        [SerializeField] private Transform _target;

        private async void OnDrawGizmos()
        {
            if (!_drawGizmos)
            {
                return;
            }

            if (_pathfinder == null || _origin == null || _target == null)
            {
                return;
            }

            var nodes = await _pathfinder.FindPath(_origin.position, _target.position, destroyCancellationToken);

            if (nodes.HasValue)
            {
                GizmoDrawPath(nodes.Value);
                nodes.Value.Dispose();
            }
        }

        public void GizmoDrawPath(NativeArray<PathfinderNode> nodes)
        {
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

            foreach (var node in nodes)
            {
                var color = Color.red;

                if (node.WaypointConnectionType == WaypointConnectionType.Fall)
                {
                    color = Color.yellow;
                }

                if (node.WaypointConnectionType == WaypointConnectionType.Walk)
                {
                    color = Color.green;
                }

                if (node.WaypointConnectionType == WaypointConnectionType.Jump)
                {
                    color = Color.blue;
                }

                Gizmos.color = color;
                Gizmos.DrawIcon(node.Position, "winbtn_mac_max@2x");
                Handles.Label(node.Position - new Vector2(0, 0.3f), $"{node.WaypointIndex.ToString()}\n{node.ParentWaypointIndex.ToString()}", style);

                if (node.ParentWaypointIndex == -1)
                {
                    continue;
                }

                Gizmos.DrawLine(node.Position, _waypoints.GetWaypointAt(node.ParentWaypointIndex).Position);
            }
        }
    }
}