using UnityEditor;
using UnityEngine;

namespace ThirdPersonTemplate
{
    [CustomEditor(typeof(Movement))]
    public class MovementEditor : Editor
    {
        private SerializedProperty spSpeed;
        private SerializedProperty spAcceleration;

        private SerializedProperty spRotationSmoothTime;

        private SerializedProperty spJumpForce;
        private SerializedProperty spGravity;
        private SerializedProperty spMaxJumpSpeed;

        private SerializedProperty spRollSpeed;

        private SerializedProperty spCrouchSpeed;

        private void OnEnable()
        {
            spSpeed = serializedObject.FindProperty("m_speed");
            spAcceleration = serializedObject.FindProperty("m_acceleration");

            spRotationSmoothTime = serializedObject.FindProperty("m_rotationSmoothTime");

            spJumpForce = serializedObject.FindProperty("m_jumpForce");
            spGravity = serializedObject.FindProperty("m_gravity");
            spMaxJumpSpeed = serializedObject.FindProperty("m_maxJumpSpeed");

            spRollSpeed = serializedObject.FindProperty("m_rollSpeed");
            spCrouchSpeed = serializedObject.FindProperty("m_crouchSpeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Locomotion", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spSpeed);
                EditorGUILayout.PropertyField(spAcceleration);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spRotationSmoothTime);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Jump", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spJumpForce);
                EditorGUILayout.PropertyField(spGravity);
                EditorGUILayout.PropertyField(spMaxJumpSpeed);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Roll", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spRollSpeed);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Crouch", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spCrouchSpeed);
            }

            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }
    }
}