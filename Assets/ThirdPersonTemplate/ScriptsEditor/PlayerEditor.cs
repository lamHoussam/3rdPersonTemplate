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

        private SerializedProperty spAimCameraSettings;

        private SerializedProperty spOnMove, spOnJump, spOnCrouch;

        private void OnEnable()
        {
            spStandCameraSettings = serializedObject.FindProperty("m_StandCameraSettings");
            spCrouchCameraSettings = serializedObject.FindProperty("m_CrouchCameraSettings");

            spLeftCameraSettings = serializedObject.FindProperty("m_LeftCameraSettings");
            spRightCameraSettings = serializedObject.FindProperty("m_RightCameraSettings");

            spAimCameraSettings = serializedObject.FindProperty("m_AimCameraSettings");

            spOnMove = serializedObject.FindProperty("m_OnMove");
            spOnJump = serializedObject.FindProperty("m_OnJump");
            spOnCrouch = serializedObject.FindProperty("m_OnCrouch");
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

                EditorGUILayout.PropertyField(spAimCameraSettings);
            }

            EditorGUILayout.EndVertical();

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