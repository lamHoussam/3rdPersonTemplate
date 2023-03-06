using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings", order = 1)]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField] private Vector2 m_offset;
        public Vector2 Offset {
            get { return m_offset; }
            set { m_offset = value; }
        }

        [SerializeField] private float m_distance;
        public float Distance
        {
            get { return m_distance; }
            set { m_distance = value; }
        }

        [SerializeField] private float m_cameraLerpTime;
        public float CameraLerpTime {
            get { return m_cameraLerpTime; }
            set { m_cameraLerpTime = value; }
        }

        [SerializeField] private Vector2 m_senstivity;
        public Vector2 Sensitivity
        {
            get { return m_senstivity; }
            set { m_senstivity = value; }
        }
    }
}