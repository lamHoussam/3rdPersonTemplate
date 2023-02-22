using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_speed, m_walkSpeed;
        [SerializeField] private float m_acceleration;

        [Space]
        [SerializeField] private float m_rotationSmoothTime;

        private float m_currentSpeed, m_targetSpeed;
        private float m_targetRotation, m_rotationVelocity;

        #region Animation IDs
        private static readonly int m_animIDSpeed = Animator.StringToHash("Speed");
        #endregion

        private CharacterController m_CharacterController;
        private Animator m_Animator;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Animator = GetComponentInChildren<Animator>();

            m_currentSpeed = m_targetSpeed = 0;
        }

        public void Move(Vector3 direction, Transform camera = null)
        {

            Rotate(direction, out Vector3 finalDirection, camera);

            m_targetSpeed = finalDirection == Vector3.zero ? 0 : m_speed;
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_acceleration * Time.deltaTime);

            m_Animator.SetFloat(m_animIDSpeed, m_currentSpeed);

            //if (m_currentSpeed <= 0.1f) return;
            if (finalDirection == Vector3.zero)
                finalDirection = transform.forward;

            m_CharacterController.Move(m_currentSpeed * Time.deltaTime * finalDirection);
        }

        public void Rotate(Vector3 inpDirection, out Vector3 finalDirection, Transform camera = null)
        {

            if (inpDirection != Vector3.zero)
            {
                m_targetRotation = Mathf.Atan2(inpDirection.x, inpDirection.z) * Mathf.Rad2Deg +
                                  (camera != null ? camera.transform.eulerAngles.y : 0);
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                    m_rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                finalDirection = Quaternion.Euler(0.0f, m_targetRotation, 0.0f) * Vector3.forward;
            }
            else
                finalDirection = Vector3.zero;
        }
    }
}