using System.Collections;
using UnityEngine;

namespace CameraSystem
{
    [System.Serializable]
    public struct CameraTriggerEvent
    {
        [SerializeField] private string m_name;
        public string Name => m_name;

        [SerializeField] private CameraSettings m_Setting;
        public CameraSettings Setting => m_Setting;

        public CameraTriggerEvent(string name, CameraSettings setting)
        {
            m_name = name;
            m_Setting = setting;
        }
    }
    public class CameraLogic : MonoBehaviour
    {
        [SerializeField] private CameraTriggerEvent[] m_triggerEvents;
        private Hashtable m_triggers = new Hashtable();

        private CameraController m_CameraController;

        private string m_currentState;
        public string CurrentState => m_currentState;

        private void Awake()
        {
            m_CameraController = GetComponent<CameraController>();
            Setup();

            SwitchCameraSetting(m_triggerEvents.Length == 0 ? "" : m_triggerEvents[0].Name);
        }

        private void Setup()
        {
            foreach (CameraTriggerEvent triggerEvent in m_triggerEvents)
                m_triggers.Add(triggerEvent.Name, triggerEvent.Setting);
        }

        public void SwitchCameraSetting(string name)
        {
            CameraSettings settings = m_triggers[name] as CameraSettings;

            if(settings == null)
            {
                Debug.LogError("No settings related to this name");
                return;
            }

            Debug.Log(settings);
            m_CameraController.BlendBetweenCameraSettings(settings);
            m_currentState = name;
        }
    }
}