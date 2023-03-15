using UnityEngine;
using NodeEditorFramework;

namespace CameraSystem
{
    [RequireComponent(typeof(CameraController))]
    public class CameraLogicGraph : MonoBehaviour
    {
        [SerializeField] private NodeCanvas m_LogicCanvas;
        private CameraController m_CameraController;

        private void Awake()
        {
            m_CameraController = GetComponent<CameraController>();
        }

        //public T GetValue<T>(string paramName) => 
        //    //m_LogicCanvas.GetValue<T>(paramName);
        //public void SetValue<T>(string paramName, T value) => m_LogicCanvas.SetValue<T>(paramName, value);

        public bool GetBool(string paramName) => m_LogicCanvas.GetBool(paramName);
        public void SetBool(string paramName, bool value, bool executeChangeImmed = true)
        {
            m_LogicCanvas.SetBool(paramName, value);
            if (executeChangeImmed)
            {
                Debug.Log(paramName + " --> value : " + value.ToString());
                CameraSettings sett = SetCameraSettingsFromGraph();
                Debug.Log(sett.name);
            }
        }

        public CameraSettings SetCameraSettingsFromGraph()
        {
            if(m_LogicCanvas == null)
            {
                Debug.LogError("Set Logic Canvas");
                return null;
            }

            CameraNode evalNode = m_LogicCanvas.Evaluate() as CameraNode;

            if(evalNode == null)
            {
                Debug.LogError("Use Camera Settings node in graph");
                return null;
            }

            m_CameraController.BlendBetweenCameraSettings(evalNode.Settings);
            return evalNode.Settings;
        }
    }
}