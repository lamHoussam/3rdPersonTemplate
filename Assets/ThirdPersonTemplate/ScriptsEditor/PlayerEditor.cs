using UnityEngine;
using UnityEditor;

namespace ThirdPersonTemplate
{
    [CustomEditor(typeof(ThirdPersonTemplate.Player))]
    public class PlayerEditor : Editor
    {
        private SerializedProperty spOnMove, spOnJump, spOnCrouch;

        private void OnEnable()
        {
            spOnMove = serializedObject.FindProperty("m_OnMove");
            spOnJump = serializedObject.FindProperty("m_OnJump");
            spOnCrouch = serializedObject.FindProperty("m_OnCrouch");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Player Events", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spOnMove);
                EditorGUILayout.PropertyField(spOnJump);
                EditorGUILayout.PropertyField(spOnCrouch);
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}