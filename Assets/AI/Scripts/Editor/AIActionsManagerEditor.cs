using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AISystem
{
    [CustomEditor(typeof(AIActionsManager))]
    public class AIActionsManagerEditor : Editor
    {
        private SerializedProperty spActionsGraphHead;

        private void OnEnable()
        {
            spActionsGraphHead = serializedObject.FindProperty("m_ActionsGraphHead");
        }

        public override void OnInspectorGUI()
        {
            AIActionsManager actionsManager = (AIActionsManager)target;
            if (GUILayout.Button("Play"))
                actionsManager.StartActionsGraph();

            EditorGUILayout.PropertyField(spActionsGraphHead);

            //EditorGUILayout.ObjectField(actionsManager.ActionsGraphHead, typeof(AIActionsManager), true);

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Actions Graph", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                GUI.enabled = false;
                AIElement element = actionsManager.ActionsGraphHead;
                while (element != null)
                {
                    EditorGUILayout.ObjectField(element, typeof(AIElement), true);
                    element = element.Next;
                }
                GUI.enabled = true;

            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}