using TMPro;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_speed, m_walkSpeed;
        [SerializeField] private float m_acceleration;

        [Space]
        [SerializeField] private float m_rotationSmoothTime;

        [Space]
        [SerializeField] private float m_jumpForce;
        [SerializeField] private float m_gravity;
        [SerializeField] private float m_maxJumpSpeed;

        private float m_currentSpeed, m_targetSpeed;
        private float m_targetRotation, m_rotationVelocity;

        private float m_verticalSpeed;

        private bool m_isJumping, m_isFalling;
        private bool m_canMove;

        private Vector3 m_planeMoveDirection;

        #region Animation IDs
        private static readonly int m_animIDSpeed = Animator.StringToHash("Speed");
        private static readonly int m_animIDJump = Animator.StringToHash("Jump");
        private static readonly int m_animIDIsFalling = Animator.StringToHash("IsFalling");
        #endregion

        private CharacterController m_CharacterController;
        private Animator m_Animator;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Animator = GetComponentInChildren<Animator>();

            m_currentSpeed = m_targetSpeed = 0;
            m_isFalling = false;
            m_isJumping = false;
            m_canMove = true;

            m_planeMoveDirection = Vector2.zero;
        }

        public void Move(Vector3 direction, Transform camera = null)
        {
            if (!m_canMove)
                return;


            Rotate(direction, out Vector3 finalDirection, camera);


            m_targetSpeed = finalDirection == Vector3.zero ? 0 : m_speed;
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_acceleration * Time.deltaTime);

            m_Animator.SetFloat(m_animIDSpeed, m_currentSpeed);

            if (!m_isFalling)
                m_planeMoveDirection = finalDirection;

            m_CharacterController.Move(m_currentSpeed * Time.deltaTime * m_planeMoveDirection + m_verticalSpeed * Time.deltaTime * Vector3.up);
        }

        public void Rotate(Vector3 inpDirection, out Vector3 finalDirection, Transform camera = null)
        {
            finalDirection = Vector3.zero;
            if (!m_canMove)
                return;

            if (inpDirection != Vector3.zero)
            {
                m_targetRotation = Mathf.Atan2(inpDirection.x, inpDirection.z) * Mathf.Rad2Deg +
                                  (camera != null ? camera.transform.eulerAngles.y : 0);
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                    m_rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                finalDirection = Quaternion.Euler(0.0f, m_targetRotation, 0.0f) * Vector3.forward;
            }
        }

        public void Jump()
        {
            if (m_isJumping || m_isFalling)
                return;

            Debug.Log("Jump now");

            m_isJumping = true;
            m_isFalling = false;
            m_Animator.SetTrigger(m_animIDJump);
        }

        public void Gravity()
        {
            if (m_CharacterController.isGrounded && m_isFalling)
            {
                m_verticalSpeed = 0;
                m_isFalling = false;
                m_isJumping = false;
                m_Animator.SetBool(m_animIDIsFalling, m_isFalling);
                return;
            }

            if (m_isJumping)
            {
                m_verticalSpeed += Time.deltaTime * m_jumpForce;
                if (m_verticalSpeed >= m_maxJumpSpeed)
                {
                    m_isFalling = true;
                    m_isJumping = false;

                    m_Animator.SetBool(m_animIDIsFalling, m_isFalling);
                }
            }


            if (m_isFalling)
                m_verticalSpeed -= Time.deltaTime * m_gravity;
        }

        private void Update()
        {
            Gravity();
        }
    }
}