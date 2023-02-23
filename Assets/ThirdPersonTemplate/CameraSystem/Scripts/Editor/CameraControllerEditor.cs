using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

namespace CameraSystem
{
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        private SerializedProperty spTarget;
        private SerializedProperty spDistance;
        private SerializedProperty spOffset;
        private SerializedProperty spCameraLerpTime;

        private SerializedProperty spActive;

        // Yaw Pitch extremes
        private SerializedProperty spMinPitchValue, spMaxPitchValue;
        private SerializedProperty spMinYawValue, spMaxYawValue;

        // Collision
        private SerializedProperty spEnableCameraCollision;
        private SerializedProperty spCameraCollisionLayer;

        private void OnEnable()
        {
            spTarget = serializedObject.FindProperty("m_Target");
            spDistance = serializedObject.FindProperty("m_distance");
            spOffset = serializedObject.FindProperty("m_offset");
            spCameraLerpTime = serializedObject.FindProperty("m_cameraLerpTime");

            spActive = serializedObject.FindProperty("m_active");

            spMinPitchValue = serializedObject.FindProperty("m_minPitchValue");
            spMaxPitchValue = serializedObject.FindProperty("m_maxPitchValue");

            spMinYawValue = serializedObject.FindProperty("m_yawMinValue");
            spMaxYawValue = serializedObject.FindProperty("m_yawMaxValue");

            spEnableCameraCollision = serializedObject.FindProperty("m_enableCameraCollision");
            spCameraCollisionLayer = serializedObject.FindProperty("m_cameraCollisionLayer");

            spEnableCameraCollision = serializedObject.FindProperty("m_enableCameraCollision");
            spCameraCollisionLayer = serializedObject.FindProperty("m_cameraCollisionLayer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(spTarget);


            // Values
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Values", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(spDistance);
                EditorGUILayout.PropertyField(spOffset);
                EditorGUILayout.PropertyField(spCameraLerpTime);
            }

            EditorGUILayout.EndVertical();


            // Angles
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Angles", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(spMinYawValue);
                EditorGUILayout.PropertyField(spMaxYawValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(spMinPitchValue);
                EditorGUILayout.PropertyField(spMaxPitchValue);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();


            // Collisions
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(spEnableCameraCollision);
            if (spEnableCameraCollision.boolValue)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(spCameraCollisionLayer);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.PropertyField(spActive);

            serializedObject.ApplyModifiedProperties();
        }
    }
}