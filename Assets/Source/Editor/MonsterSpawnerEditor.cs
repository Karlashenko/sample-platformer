using Sample.Systems;
using UnityEditor;
using UnityEngine;

namespace Sample.Editor
{
    [CustomEditor(typeof(MonsterSpawner))]
    public class MonsterSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(25);

            if (!GUILayout.Button("Refresh"))
            {
                return;
            }

            ((MonsterSpawner) target).Refresh();
        }
    }
}