using System.Collections;
using UnityEngine;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        [SerializeField] private float m_distance;
        [SerializeField] private Vector2 m_offset;

        [SerializeField] private float m_cameraLerpTime;

        private float m_targetDistance;
        private Vector3 m_expectedPos, m_realOffset;

        [SerializeField] private float m_minPitchValue = -10, m_maxPitchValue = 70;
        [SerializeField] private float m_yawMinValue = -50, m_yawMaxValue = 50;

        [SerializeField] private bool m_enableCameraCollision;
        [SerializeField] private LayerMask m_cameraCollisionLayer;

        [SerializeField] private bool m_active;
        public bool Active => m_active;


        private float m_yaw, m_pitch;
        private void LateUpdate()
        {
            if (!m_Target || !Active) return;
            SetPitchYaw();
            ThirdPersonCamera();
        }

        public void SetPitchYaw()
        {
            m_pitch += Input.GetAxis("Mouse Y");
            m_yaw += Input.GetAxis("Mouse X");

            m_pitch = ClampAngle(m_pitch, m_minPitchValue, m_maxPitchValue);
            m_yaw = ClampAngle(m_yaw, m_yawMinValue, m_yawMaxValue);

        }

        private void ThirdPersonCamera()
        {
            m_targetDistance = m_distance;
            //if (m_Player)
            //    m_targetDistance = m_Player.Aiming ? m_aimingDistance : m_normalDistance;
            m_distance = Mathf.Lerp(m_distance, m_targetDistance, Time.deltaTime * 5);

            float hor = -m_distance * Mathf.Cos(m_pitch * Mathf.Deg2Rad);


            Vector3 t = m_expectedPos - m_Target.position;
            Vector3 dir = RotateVector(t, 90).normalized;

            m_realOffset = dir * m_offset.x + Vector3.up * m_offset.y;

            Vector3 pos = m_Target.position + new Vector3(
                hor * Mathf.Sin(m_yaw * Mathf.Deg2Rad),
                m_distance * Mathf.Sin(m_pitch * Mathf.Deg2Rad),
                hor * Mathf.Cos(m_yaw * Mathf.Deg2Rad)
            );
            Quaternion rot = Quaternion.Euler(/*(Vector3)m_offset + */Vector3.up * m_yaw + Vector3.right * m_pitch);

            m_expectedPos = pos;

            Vector3 direction = (pos - m_Target.position).normalized;

            Ray ray = new Ray(m_Target.position + m_realOffset, direction);

            Vector3 finalTargetPosition;


            if (m_enableCameraCollision && Physics.Raycast(ray, out RaycastHit hit, m_distance, m_cameraCollisionLayer))
                finalTargetPosition = hit.point - direction * .3f;
            else
                finalTargetPosition = pos + m_realOffset;

            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, finalTargetPosition, m_cameraLerpTime * Time.deltaTime),
                Quaternion.Lerp(transform.rotation, rot, m_cameraLerpTime * Time.deltaTime));

        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public static Vector3 RotateVector(Vector3 vec, float angle)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);

            return new Vector3(
                vec.x * cos - vec.z * sin,
                0,
                vec.x * sin + vec.z * cos
            );
        }
    }
}