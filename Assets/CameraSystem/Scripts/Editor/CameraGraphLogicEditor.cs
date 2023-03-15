using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CameraSystem
{
    [CustomEditor(typeof(CameraLogicGraph))]
    public class CameraGraphLogicEditor : Editor
    {

        private CameraLogicGraph m_logic;

        private bool m_crouched = false;
        private bool m_aim = false;

        private void OnEnable()
        {
            m_logic = (CameraLogicGraph)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Switch"))
                m_logic.SetCameraSettingsFromGraph();

            if (GUILayout.Button("Crouch"))
            {
                m_crouched = !m_crouched;
                m_logic.SetBool("isCrouched", m_crouched);
            }

            if (GUILayout.Button("Aim"))
            {
                m_aim = !m_aim;
                m_logic.SetBool("aim", m_aim);
            }
        }
    }
}