using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CameraSystem
{
    [CustomEditor(typeof(CameraSequence))]
    public class CameraSequenceEditor : Editor
    {
        private CameraSequence m_targetObject;

        private void OnEnable()
        {
            m_targetObject = (CameraSequence)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Start Sequence"))
                m_targetObject.StartSequence();
        }
    }
}