using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings", order = 1)]
public class CameraSettings : ScriptableObject
{
    [SerializeField] private Vector2 m_offset;
    public Vector2 Offset => m_offset;

    [SerializeField] private float m_distance;
    public float Distance => m_distance;

    [SerializeField] private float m_cameraLerpTime;
    public float CameraLerpTime => m_cameraLerpTime;
}
