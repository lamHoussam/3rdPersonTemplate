using UnityEditor;
using UnityEngine;

namespace ThirdPersonTemplate
{
    [CustomEditor(typeof(Movement))]
    public class MovementEditor : Editor
    {
        private SerializedProperty spSpeed;
        private SerializedProperty spWalkSpeed;
        private SerializedProperty spAcceleration;


        private SerializedProperty spRotationSmoothTime;

        private SerializedProperty spJumpForce;
        private SerializedProperty spGravity;
        private SerializedProperty spMaxJumpSpeed;

        private SerializedProperty spRollSpeed;

        private SerializedProperty spCrouchSpeed;

        private SerializedProperty spCrouchHeight, spCrouchCenter;
        private SerializedProperty spStandHeight, spStandCenter;


        private SerializedProperty spInCoverSpeed;

        private SerializedProperty spActivateJump;
        private SerializedProperty spActivateRoll;
        private SerializedProperty spActivateCrouch;
        private SerializedProperty spActivateSwim;

        public virtual void OnEnable()
        {
            spSpeed = serializedObject.FindProperty("m_speed");
            spWalkSpeed = serializedObject.FindProperty("m_walkSpeed");
            spAcceleration = serializedObject.FindProperty("m_acceleration");

            spRotationSmoothTime = serializedObject.FindProperty("m_rotationSmoothTime");

            spJumpForce = serializedObject.FindProperty("m_jumpForce");
            spGravity = serializedObject.FindProperty("m_gravity");
            spMaxJumpSpeed = serializedObject.FindProperty("m_maxJumpSpeed");

            spRollSpeed = serializedObject.FindProperty("m_rollSpeed");
            spCrouchSpeed = serializedObject.FindProperty("m_crouchSpeed");

            spCrouchHeight = serializedObject.FindProperty("m_crouchHeight");
            spCrouchCenter = serializedObject.FindProperty("m_crouchCenter");
            spStandHeight = serializedObject.FindProperty("m_standHeight");
            spStandCenter = serializedObject.FindProperty("m_standCenter");

            spInCoverSpeed = serializedObject.FindProperty("m_inCoverSpeed");


            spActivateJump = serializedObject.FindProperty("m_activateJump");
            spActivateCrouch = serializedObject.FindProperty("m_activateCrouch");
            spActivateRoll = serializedObject.FindProperty("m_activateRoll");
            spActivateSwim = serializedObject.FindProperty("m_ativateSwim");
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
                EditorGUILayout.PropertyField(spWalkSpeed);
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

            EditorGUILayout.PropertyField(spActivateJump);
            if (spActivateJump.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(spJumpForce);
                    EditorGUILayout.PropertyField(spGravity);
                    EditorGUILayout.PropertyField(spMaxJumpSpeed);
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Roll", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(spActivateRoll);

            if(spActivateRoll.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(spRollSpeed);
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Crouch", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(spActivateCrouch);

            if (spActivateCrouch.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(spCrouchSpeed);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(spCrouchHeight);
                    EditorGUILayout.PropertyField(spCrouchCenter);
                    EditorGUILayout.PropertyField(spStandHeight);
                    EditorGUILayout.PropertyField(spStandCenter);
                }
            }


            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Cover", EditorStyles.boldLabel);

            //EditorGUILayout.Prop
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spInCoverSpeed);
            }
            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();
        }
    }
}