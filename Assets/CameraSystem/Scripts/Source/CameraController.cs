using UnityEngine;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        [SerializeField] private float m_distance;
        [SerializeField] private Vector2 m_offset;

        [SerializeField] private string m_assetName, m_assetPath;

        [SerializeField] private float m_cameraLerpTime;

        [SerializeField] private Vector2 m_sensitivity;

        private float m_targetDistance;
        private Vector3 m_expectedPos, m_realOffset;

        [SerializeField] private bool m_useYawLimit, m_usePitchLimit;
        [SerializeField] private float m_minPitchValue = -10, m_maxPitchValue = 70;
        [SerializeField] private float m_yawMinValue = -50, m_yawMaxValue = 50;

        [SerializeField] private bool m_enableCameraCollision;
        [SerializeField] private LayerMask m_cameraCollisionLayer;

        [SerializeField] private bool m_active;
        public bool Active => m_active;

        [SerializeField] private float m_transitionTime;
        [SerializeField] private AnimationCurve m_TransitionCurve;

        [SerializeField] private CameraSettings m_CameraSettingsToLoad;

        [SerializeField] private Transform m_TargetLockOn;

        private bool m_isBlending;

        private float m_blendDistanceVariation;
        private Vector2 m_blendOffsetVariation;
        private float t;

        private CameraSettings m_TargetSettings;

        [SerializeField] private float m_yaw, m_pitch;

        //private float m_distVar;
        private float m_previousDistance;
        private Vector2 m_previousOffset;

        private bool m_isLockedOnTarget;

        private void Awake()
        {
            m_previousDistance = m_distance;
            m_previousOffset = m_offset;

            //ActivateLockOn(m_TargetLockOn);
        }

        public void LateUpdate()
        {
            if (!m_Target || !Active) return;

#if ENABLE_LEGACY_INPUT_MANAGER
            SetPitchYaw();
#endif

            if (m_isLockedOnTarget)
                LockOn();
            else
                ThirdPersonCamera();
        }

        private void Update()
        {
            if (m_isBlending)
                Blend();
        }


        /// <summary>
        /// Change camera's pitch and yaw 
        /// </summary>
        /// <param name="look">Pitch, Yaw values to add </param>
        public void SetPitchYaw(Vector2 look)
        {
            if (m_isLockedOnTarget)
                return;


            m_pitch += Time.deltaTime * look.y * m_sensitivity.y;
            m_yaw += Time.deltaTime * look.x * m_sensitivity.x;

            m_pitch = ClampAngle(m_pitch, m_minPitchValue, m_maxPitchValue);
            if (m_useYawLimit)
                m_yaw = ClampAngle(m_yaw, m_yawMinValue, m_yawMaxValue);
        }

        public void SetPitchYaw(float x, float y) => SetPitchYaw(new Vector2(x, y));

#if ENABLE_LEGACY_INPUT_MANAGER
        public void SetPitchYaw() => SetPitchYaw(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
#endif
        /// <summary>
        /// Third Person Camera
        /// </summary>
        public void ThirdPersonCamera()
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
            
            // TODO: Change to take direction from player
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

        public void ActivateLockOn(Transform targetLock)
        {
            m_isLockedOnTarget = true;

            m_TargetLockOn = targetLock;
        }

        private void LockOn(Transform targetLock)
        {
            Vector3 dir = m_Target.position - targetLock.position;
            Vector3 camPosition = dir + dir.normalized * m_distance;
            m_realOffset = transform.right * m_offset.x + Vector3.up * m_offset.y;

            transform.position = targetLock.position + camPosition + m_realOffset;
            transform.LookAt(targetLock.position);
        }

        private void LockOn() => LockOn(m_TargetLockOn);

        public void DeactivateLockOn()
        {
            m_isLockedOnTarget = false;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        /// <summary>
        /// Rotate vector vec angle degrees
        /// </summary>
        /// <param name="vec">Vector to rotate</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Rotation result</returns>
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

        /// <summary>
        /// Set Camera new settings
        /// </summary>
        /// <param name="settings"></param>
        public void SetCameraSettings(CameraSettings settings)
        {
            m_offset = settings.Offset;
            m_distance = settings.Distance;
            m_cameraLerpTime = settings.CameraLerpTime;
            StopBlend();
        }

        /// <summary>
        /// Initiate Camera blend to new settings
        /// </summary>
        /// <param name="settings"></param>
        public void BlendBetweenCameraSettings(CameraSettings settings)
        {
            m_isBlending = true;
            m_TargetSettings = settings;

            m_cameraLerpTime = settings.CameraLerpTime;

            m_blendDistanceVariation = settings.Distance - m_distance;
            m_blendOffsetVariation = settings.Offset - m_offset;

            m_previousDistance = m_distance;
            m_previousOffset = m_offset;

            t = 0;
        }

        private void Blend()
        {
            t += Time.deltaTime;

            float val = m_TransitionCurve.Evaluate(t / m_transitionTime);

            m_distance = val * m_blendDistanceVariation + m_previousDistance;
            m_offset = val * m_blendOffsetVariation + m_previousOffset;

            Debug.LogWarning("Value : " + val);

            if (t >= m_transitionTime)
                SetCameraSettings(m_TargetSettings);
        }

        /// <summary>
        /// Stop Camera blend
        /// </summary>
        public void StopBlend()
        {
            m_isBlending = false;
        }
    }
}