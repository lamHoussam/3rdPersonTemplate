using UnityEngine;
using NodeEditorFramework;
using UnityEditor;

namespace CameraSystem
{
    public class CameraNode : Node
    {
        #region Members
        [SerializeField] private CameraSettings m_CameraSettings;
        public CameraSettings Settings => m_CameraSettings;
        #endregion Members

        public static CameraNode Create(Rect rect)
        {
            CameraNode node = CreateInstance<CameraNode>();

            node.m_Rect = rect;
            node.m_InitialRect = rect;

            node.Init();

            return node;

        }

        public override void Draw(float scale = 1)
        {

            base.Draw(scale);
            GUILayout.BeginArea(m_Rect, m_isEvaluationResult ? NodeEditor.Instance.m_EvaluatedNodeResult : NodeEditor.Instance.m_NodeBox);

            GUILayout.Label(Settings ? Settings.name : "CameraSettings");
            m_CameraSettings = (CameraSettings)EditorGUILayout.ObjectField(m_CameraSettings, typeof(CameraSettings), false);
            GUILayout.EndArea();

        }

        public override void OnRemove()
        {

            base.OnRemove();
        }
    }
}