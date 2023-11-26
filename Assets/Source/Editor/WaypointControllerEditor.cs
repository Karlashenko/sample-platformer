using Sample.Pathfinding;
using UnityEditor;
using UnityEngine;

namespace Sample.Editor
{
    [CustomEditor(typeof(Waypoints))]
    public class WaypointControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(25);

            if (!GUILayout.Button("Generate"))
            {
                return;
            }

            ((Waypoints) target).Generate();
        }
    }
}