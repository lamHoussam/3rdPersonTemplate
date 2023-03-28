using UnityEngine;
using NodeEditorFramework;
using UnityEditor;

namespace CameraSystem
{
    public class CameraSettingsNode : Node
    {
        #region Members
        [SerializeField] private CameraSettings m_Settings;
        public CameraSettings Settings => m_Settings;
        #endregion Members

        public static CameraSettingsNode Create(Rect rect)
        {
            CameraSettingsNode node = CreateInstance<CameraSettingsNode>();

            node.m_Rect = rect;
            node.Init();

            return node;

        }

        public override void Draw(float scale = 1)
        {
            GUILayout.BeginArea(m_Rect, m_isEvaluationResult ? NodeEditor.Instance.m_EvaluatedNodeResult : NodeEditor.Instance.m_NodeBox);

            GUILayout.Label(m_Settings ? m_Settings.name : "Settings");
            m_Settings = (CameraSettings)EditorGUILayout.ObjectField(m_Settings, typeof(CameraSettings), false);
            GUILayout.EndArea();

            base.Draw(scale);
        }

        public override void OnRemove()
        {

        }
    }
}