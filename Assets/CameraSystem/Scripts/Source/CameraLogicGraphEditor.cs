
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;

namespace CameraSystem
{
    [CustomEditor(typeof(CameraLogicGraph))]
    public class CameraLogicGraphEditor : Editor
    {
        private NodeCanvas m_Cnv;
        private void OnEnable()
        {

            m_Cnv = serializedObject.FindProperty("m_LogicCanvas").objectReferenceValue as NodeCanvas;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (!m_Cnv)
                return;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Graph parameters");

            for(int i = 0; i < m_Cnv.ParametersCount; i++)
            {
                NodeEditorParameter param = m_Cnv.GetParameter(i);
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(param.Name);
                EditorGUILayout.Toggle(param.Value.BoolValue);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);

            for(int i = 0; i < m_Cnv.NodeConnectionsCount; i++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                NodeConnection cnx = m_Cnv.GetNodeConnection(i);
                for(int j = 0; j < cnx.ConditionsCount; j++)
                {
                    ConnectionCondition cnd = cnx.GetCondition(j);
                    EditorGUILayout.BeginHorizontal();

                    //EditorGUILayout.LabelField(cnd.);
                    //EditorGUILayout.Toggle(param.Value.BoolValue);

                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }
    }
}