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

            m_LogicCanvas.LoadCanvasParameterState();
            //Debug.LogWarning(m_LogicCanvas.GetParameter("crouch"));
            //m_LogicCanvas.SaveCanvasParameterState();
            //m_LogicCanvas.LoadCanvasParameterState();
        }

        //public T GetValue<T>(string paramName) => 
        //    //m_LogicCanvas.GetValue<T>(paramName);
        //public void SetValue<T>(string paramName, T value) => m_LogicCanvas.SetValue<T>(paramName, value);

        /// <summary>
        /// Get boolean value of parameter with paramName from NodeCanvas graph
        /// </summary>
        /// <param name="paramName">Parameter's name</param>
        /// <returns>Parameter's value</returns>
        public bool GetBool(string paramName) => m_LogicCanvas.GetBool(paramName);

        /// <summary>
        /// Set boolean value of parameter with parameterName
        /// </summary>
        /// <param name="paramName">Parameter's name</param>
        /// <param name="value">Parameter's new value</param>
        /// <param name="executeChangeImmed">Apply evaluated camera Settings immediately</param>
        public void SetBool(string paramName, bool value, bool executeChangeImmed = true)
        {
            m_LogicCanvas.SetBool(paramName, value);
            if (executeChangeImmed)
                SetCameraSettingsFromGraph();
        }

        /// <summary>
        /// Evaluate graph and get camera settings then set them to CameraController
        /// </summary>
        /// <param name="blend">if true blend to settings if false change immediately</param>
        /// <returns>Camera settings to set camera to</returns>
        public CameraSettings SetCameraSettingsFromGraph(bool blend = true)
        {
            if(m_LogicCanvas == null)
            {
                Debug.LogError("Set Logic Canvas");
                return null;
            }

            CameraNode evalNode = m_LogicCanvas.Evaluate() as CameraNode;
            if(evalNode == default)
            {
                Debug.LogError("Use Camera Settings node in graph");
                return default(CameraSettings);
            }
            if(blend)
                m_CameraController.BlendBetweenCameraSettings(evalNode.Settings);
            else
                m_CameraController.SetCameraSettings(evalNode.Settings);

            Debug.LogError(evalNode.Settings);
            return evalNode.Settings;
        }
    }
}