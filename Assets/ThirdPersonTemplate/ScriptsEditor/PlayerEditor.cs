using UnityEngine;
using UnityEditor;

namespace ThirdPersonTemplate
{
    [CustomEditor(typeof(ThirdPersonTemplate.Player))]
    public class PlayerEditor : Editor
    {

        private SerializedProperty spStandCameraSettings;
        private SerializedProperty spCrouchCameraSettings;

        private SerializedProperty spLeftCameraSettings;
        private SerializedProperty spRightCameraSettings;

        private void OnEnable()
        {
            spStandCameraSettings = serializedObject.FindProperty("m_StandCameraSettings");
            spCrouchCameraSettings = serializedObject.FindProperty("m_CrouchCameraSettings");

            spLeftCameraSettings = serializedObject.FindProperty("m_LeftCameraSettings");
            spRightCameraSettings = serializedObject.FindProperty("m_RightCameraSettings");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Settings
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spStandCameraSettings);
                EditorGUILayout.PropertyField(spCrouchCameraSettings);
                EditorGUILayout.PropertyField(spLeftCameraSettings);
                EditorGUILayout.PropertyField(spRightCameraSettings);

            }

            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }
    }
}