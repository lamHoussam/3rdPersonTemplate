using TMPro;
using UnityEngine;
using static UnityEngine.LightAnchor;

namespace ThirdPersonTemplate
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_speed, m_walkSpeed;
        [SerializeField] private float m_acceleration;

        [SerializeField] private float m_rotationSmoothTime;

        [SerializeField] private float m_jumpForce;
        [SerializeField] private float m_gravity;
        [SerializeField] private float m_maxJumpSpeed;

        [SerializeField] private float m_rollSpeed;

        [SerializeField] private float m_crouchSpeed;

        private float m_currentSpeed, m_targetSpeed;
        private float m_targetRotation, m_rotationVelocity;

        private float m_verticalSpeed;

        private bool m_isJumping, m_isFalling;
        private bool m_canMove, m_canJump;
        private bool m_isRolling;

        private bool m_isCrouched;
        public bool IsCrouched => m_isCrouched;

        private Vector3 m_planeMoveDirection;

        #region Animation IDs
        private static readonly int m_animIDSpeed = Animator.StringToHash("Speed");
        private static readonly int m_animIDJump = Animator.StringToHash("Jump");
        private static readonly int m_animIDIsFalling = Animator.StringToHash("IsFalling");
        private static readonly int m_animIDRoll = Animator.StringToHash("Roll");
        private static readonly int m_animIDCrouch = Animator.StringToHash("Crouch");
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
            m_isRolling = false;
            m_isCrouched = false;


            m_canMove = true;
            m_canJump = true;


            m_planeMoveDirection = Vector2.zero;
        }

        public void Move(Vector3 direction, Transform camera = null)
        {
            if (!m_canMove)
            {
                //Debug.Log("Cant MOve");
                return;
            }


            Rotate(direction, out Vector3 finalDirection, camera);

            m_targetSpeed = m_isCrouched ? m_crouchSpeed : m_speed;
            m_targetSpeed = finalDirection == Vector3.zero ? 0 : m_targetSpeed;

            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_acceleration * Time.deltaTime);

            m_Animator.SetFloat(m_animIDSpeed, m_currentSpeed);

            if (!m_isFalling && !m_isJumping)
                m_planeMoveDirection = finalDirection;

            m_CharacterController.Move(m_currentSpeed * Time.deltaTime * m_planeMoveDirection + m_verticalSpeed * Time.deltaTime * Vector3.up);
        }

        public void Rotate(Vector3 inpDirection, out Vector3 finalDirection, Transform camera = null)
        {
            finalDirection = (m_isJumping || m_isFalling) ? m_planeMoveDirection : Vector3.zero;

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
            if (m_isJumping || m_isFalling || !m_canJump)
                return;

            Debug.Log("Jump now");

            m_isJumping = true;
            //m_isFalling = false;
            m_Animator.SetTrigger(m_animIDJump);
        }

        public void Roll(Vector3 rollDirection, Transform camera = null)
        {
            if (m_isJumping || m_isFalling)
                return;

            Vector3 dir;
            if (rollDirection == Vector3.zero)
                dir = camera != null ? camera.transform.forward : transform.forward;
            else
                Rotate(rollDirection, out dir, camera);


            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            m_Animator.SetTrigger(m_animIDRoll);
            m_canMove = false;
            m_isRolling = true;
        }

        public void Roll() => Roll(transform.forward);

        private void ManageRoll()
        {
            if (!m_isRolling)
                return;

            m_CharacterController.Move(Time.deltaTime * m_rollSpeed * transform.forward + Time.deltaTime * m_verticalSpeed * Vector3.up);
        }

        public void OnStopRoll()
        {
            m_isRolling = false;
            m_canMove = true;
        }

        public void Gravity()
        {
            //if (m_CharacterController.isGrounded)
            //{
            //    m_verticalSpeed = 0;
            //    m_isFalling = false;
            //    m_isJumping = false;
            //    m_Animator.SetBool(m_animIDIsFalling, false);
            //    //return;
            //}

            m_isFalling = !m_CharacterController.isGrounded;

            if (m_isJumping)
            {
                m_verticalSpeed += Time.deltaTime * m_jumpForce;
                if (m_verticalSpeed >= m_maxJumpSpeed)
                {
                    m_isFalling = true;
                    m_isJumping = false;

                    m_Animator.SetBool(m_animIDIsFalling, true);
                }

                return;

            }


            if (m_isFalling)
                m_verticalSpeed -= Time.deltaTime * m_gravity;
            else 
                m_verticalSpeed = -0.1f;

            m_Animator.SetBool(m_animIDIsFalling, m_isFalling);
        }

        public void Crouch()
        {
            if (m_isJumping || m_isFalling)
                return;

            m_isCrouched = !m_isCrouched;
            m_Animator.SetBool(m_animIDCrouch, m_isCrouched);

            m_canJump = !m_isCrouched;
        }

        public void DeactivateMovement()
        {
            m_canMove = false;
        }

        public void ActivateMovement()
        {
            m_canMove = true;
        }

        private void Update()
        {
            Gravity();
            ManageRoll();
        }
    }
}