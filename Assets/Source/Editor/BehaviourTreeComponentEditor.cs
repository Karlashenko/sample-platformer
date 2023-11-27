using System;
using System.Linq;
using Sample.AI;
using Sample.Components.Entities;
using UnityEditor;
using UnityEngine;

namespace Sample.Editor
{
    [CustomEditor(typeof(BehaviourTreeComponent))]
    public class BehaviourTreeComponentEditor : UnityEditor.Editor
    {
        private BehaviourTreeComponent? _behaviourTree;

        private void OnEnable()
        {
            _behaviourTree = target as BehaviourTreeComponent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_behaviourTree?.BehaviourTree == null)
            {
                return;
            }

            GUILayout.Space(25);
            EditorGUILayout.LabelField("Target Entity", _behaviourTree.BehaviourTree.Context.TargetUnit == null ? "Null" : _behaviourTree.BehaviourTree.Context.TargetUnit.name);
            EditorGUILayout.LabelField("Target Point", _behaviourTree.BehaviourTree.Context.TargetPoint.ToString());
            EditorGUILayout.LabelField("Opened Nodes", string.Join(", ", _behaviourTree.BehaviourTree.Context.RunningNodes.Select(node => node.GetType().Name)));
            GUILayout.Space(25);
            DisplayParentNode(_behaviourTree.BehaviourTree, _behaviourTree.BehaviourTree.Root, 0);
            GUILayout.Space(25);
        }

        private void DisplayParentNode(BehaviourTree tree, BehaviourTreeCompositeNode node, int level)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                richText = true,
                padding = new RectOffset(10 * level, 0, 0, 0),
                fontStyle = tree.GetFirstRunningNode().Equals(node) ? FontStyle.Bold : FontStyle.Normal
            };

            GUILayout.Label($"<color=#{GetColorByStatus(node.GetLastStatus())}>{node.GetType().Name}</color>", style);

            foreach (var child in node.Nodes)
            {
                if (child is BehaviourTreeCompositeNode subParent)
                {
                    DisplayParentNode(tree, subParent, level + 1);
                    continue;
                }

                style.padding = new RectOffset(10 * (level + 1), 0, 0, 0);
                style.fontStyle = tree.GetFirstRunningNode().Equals(child) ? FontStyle.Bold : FontStyle.Normal;

                GUILayout.Label($"<color=#{GetColorByStatus(child.GetLastStatus())}>{child.GetType().Name}</color>", style);
            }
        }

        private static string GetColorByStatus(BehaviourTreeStatus status)
        {
            switch (status)
            {
                case BehaviourTreeStatus.Success:
                    return "008800";
                case BehaviourTreeStatus.Failure:
                    return "880000";
                case BehaviourTreeStatus.Running:
                    return "000088";
                case BehaviourTreeStatus.None:
                    return "333333";
                case BehaviourTreeStatus.Waiting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "ff00ff";
        }
    }
}
