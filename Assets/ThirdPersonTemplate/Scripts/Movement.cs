using CameraSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonTemplate
{
    public class Movement : MonoBehaviour
    {

        // Basic Movement
        [SerializeField] protected float m_speed, m_walkSpeed;
        [SerializeField] protected float m_acceleration;

        [SerializeField] protected float m_rotationSmoothTime;

        // Jump
        [SerializeField] protected float m_jumpForce;
        [SerializeField] protected float m_gravity;
        [SerializeField] protected float m_maxJumpSpeed;

        [SerializeField] protected bool m_activateJump;


        // Roll
        [SerializeField] protected float m_rollSpeed;

        [SerializeField] protected bool m_activateRoll;


        // Crouch
        [SerializeField] protected float m_crouchSpeed;

        [SerializeField] protected float m_crouchHeight, m_standHeight;
        [SerializeField] protected float m_crouchCenter, m_standCenter;

        [SerializeField] protected bool m_activateCrouch;

        // Swim
        [SerializeField] protected float m_swimSpeed;
        [SerializeField] protected float m_buoyantForce;

        [SerializeField] protected bool m_activateSwim;

        // Cover
        [SerializeField] protected float m_inCoverSpeed;

        protected float m_currentSpeed, m_targetSpeed;
        protected float m_targetRotation, m_rotationVelocity;

        protected float m_verticalSpeed;

        protected bool m_isJumping, m_isFalling;
        protected bool m_canMove, m_canJump;
        protected bool m_isRolling;
        protected bool m_isSwimming;

        protected bool m_inCover;
        public bool InCover => m_inCover;

        protected bool m_isCrouched;
        public bool IsCrouched => m_isCrouched;

        protected Vector3 m_planeMoveDirection;

        #region Animation IDs
        protected static readonly int m_animIDSpeed = Animator.StringToHash("Speed");
        protected static readonly int m_animIDJump = Animator.StringToHash("Jump");
        protected static readonly int m_animIDIsFalling = Animator.StringToHash("IsFalling");
        protected static readonly int m_animIDRoll = Animator.StringToHash("Roll");
        protected static readonly int m_animIDCrouch = Animator.StringToHash("Crouch");
        protected static readonly int m_animIDSwimming = Animator.StringToHash("Swimming");
        protected static readonly int m_animIDInCover = Animator.StringToHash("InCover");
        protected static readonly int m_animIDCoverDirection = Animator.StringToHash("CoverDirection");
        protected static readonly int m_animIDClimb = Animator.StringToHash("Climb");
        #endregion

        protected CharacterController m_CharacterController;
        protected Animator m_Animator;

        protected PlayerRaycaster m_PlayerRaycaster;
        protected Player m_Player;

        private CameraLogic m_CameraLogic;

        public virtual void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Animator = GetComponentInChildren<Animator>();
            m_PlayerRaycaster = GetComponent<PlayerRaycaster>();
            m_Player = GetComponent<Player>();

            m_CameraLogic = Camera.main.GetComponent<CameraLogic>();

            m_currentSpeed = m_targetSpeed = 0;
            m_isFalling = false;
            m_isJumping = false;
            m_isRolling = false;
            m_isCrouched = false;


            m_canMove = true;
            m_canJump = true;


            m_planeMoveDirection = Vector2.zero;
        }

        private void CoverMove(Vector3 direction)
        {
            float val = direction.x == 0 ? 0 : -Mathf.Sign(direction.x);

            if (direction.z != 0)
            {
                LeaveCover();
                return;
            }

            bool checkCanMove = true;
            if (val > 0)
            {
                checkCanMove = m_PlayerRaycaster.CanGoLeftCover(-transform.forward);
                m_Animator.SetFloat(m_animIDCoverDirection, val);
                if (m_CameraLogic.CurrentState != "coverRight")
                    m_CameraLogic.SwitchCameraSetting("coverRight");
            }
            else if (val < 0)
            {
                checkCanMove = m_PlayerRaycaster.CanGoRightCover(-transform.forward);
                m_Animator.SetFloat(m_animIDCoverDirection, val);
                if (m_CameraLogic.CurrentState != "coverLeft")
                    m_CameraLogic.SwitchCameraSetting("coverLeft");
            }



            m_targetSpeed = checkCanMove && val != 0 ? m_inCoverSpeed : 0;

            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_acceleration * Time.deltaTime);

            Vector3 movement = m_currentSpeed * Time.deltaTime * val * transform.right + m_verticalSpeed * Time.deltaTime * Vector3.up;
            Debug.DrawRay(transform.position, movement, Color.yellow, 10);
            m_CharacterController.Move(movement);


            m_Animator.SetFloat(m_animIDSpeed, m_currentSpeed);
        }

        public virtual void Move(Vector3 direction, bool isRunning = false, Transform camera = null)
        {
            if (!m_canMove)
            {
                //Debug.Log("Cant MOve");
                return;
            }


            Rotate(direction, out Vector3 finalDirection, camera);



            if (InCover)
            {
                CoverMove(finalDirection);
                return;
            }

            if (m_isSwimming)
            {
                Vector3 movement = m_currentSpeed * Time.deltaTime * finalDirection + m_verticalSpeed * Time.deltaTime * Vector3.up;
                m_CharacterController.Move(movement);
                Debug.LogError(movement);

                return;
            }

            m_targetSpeed = m_isCrouched ? m_crouchSpeed : m_walkSpeed;
            m_targetSpeed = isRunning ? m_speed : m_targetSpeed;
            m_targetSpeed = finalDirection == Vector3.zero ? 0 : m_targetSpeed;

            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_acceleration * Time.deltaTime);

            m_Animator.SetFloat(m_animIDSpeed, m_currentSpeed);

            if (!m_isFalling && !m_isJumping)
                m_planeMoveDirection = finalDirection;

            Vector3 horizontalMotion = m_currentSpeed * Time.deltaTime * m_planeMoveDirection;
            m_CharacterController.Move(horizontalMotion + m_verticalSpeed * Time.deltaTime * Vector3.up);

            if (m_Player.m_OnMove != null && horizontalMotion != Vector3.zero)
            {
                m_Player.m_OnMove?.Invoke();
                //m_Player.m_OnMove = null;
                //m_Player.m_OnMove.RemoveAllListeners();
            }
        }

        public virtual void Rotate(Vector3 inpDirection, out Vector3 finalDirection, Transform camera = null)
        {
            finalDirection = (m_isJumping || m_isFalling) && !m_isSwimming ? m_planeMoveDirection : Vector3.zero;

            if (!m_canMove)
                return;

            if (InCover)
            {
                finalDirection = inpDirection;
                return;
            }

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
            if (!m_activateJump || m_isJumping || m_isFalling || !m_canJump)
                return;

            m_isJumping = true;
            //m_isFalling = false;
            if(m_Player.m_OnJump != null)
            {
                m_Player.m_OnJump.Invoke();
                //m_Player.m_OnJump = null;
            }
            m_Animator.SetTrigger(m_animIDJump);
        }

        public void Roll(Vector3 rollDirection, Transform camera = null)
        {
            if (!m_activateRoll || m_isJumping || m_isFalling)
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
            m_canJump = false;
            m_isRolling = true;

            SetCharacterControllerHeightCenter();
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
            Debug.LogWarning(m_Animator.GetBool(m_animIDCrouch));

            m_isRolling = false;
            m_canMove = true;
            m_canJump = true;

            CrouchStand();
        }

        public void Gravity()
        {

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

            if (m_Animator.GetBool(m_animIDIsFalling) != m_isFalling)
                m_Animator.SetBool(m_animIDIsFalling, m_isFalling);
        }


        private bool Stand()
        {
            if (!m_PlayerRaycaster.CanStand())
                return false;
            m_isCrouched = false;

            m_Animator.SetBool(m_animIDCrouch, m_isCrouched);
            m_CameraLogic.SwitchCameraSetting(m_Player.RightShoulder ? "rightStand" : "leftStand");
            SetCharacterControllerHeightCenter();

            return true;
        }

        private void Crouch()
        {
            m_isCrouched = true;

            DeactivateJump();
            DeactivateMovement();

            m_Animator.SetBool(m_animIDCrouch, m_isCrouched);
            m_CameraLogic.SwitchCameraSetting("crouch");

            if (m_Player.m_OnCrouch != null)
            {
                m_Player.m_OnCrouch?.Invoke();
                //m_Player.m_OnCrouch = null;
            }
            SetCharacterControllerHeightCenter();
        }

        public void CrouchStand()
        {

            if (!m_activateCrouch) 
                return;

            if (!m_isCrouched)
            {
                bool val = Stand();
                if (!val)
                    Crouch();
            }

            SetCharacterControllerHeightCenter();
        }

        public void ChangeCrouchStandState()
        {
            if (!m_activateCrouch || m_isJumping || m_isFalling)
                return;


            if (m_isCrouched)
                Stand();
            else
                Crouch();

        }

        public void OnStartSwimming()
        {
            if (!m_activateSwim) 
                return;

            m_isSwimming = true;
            m_Animator.SetBool(m_animIDSwimming, m_isSwimming);
            Debug.Log("Start swimming");
        }

        public void ApplySwimForces()
        {
        }

        public void OnStopSwimming()
        {
            m_isSwimming = false;
            m_Animator.SetBool(m_animIDSwimming, m_isSwimming);
            Debug.Log("Stop swimming");
            m_verticalSpeed += (m_gravity + .1f) * Time.deltaTime;
        }


        public void TakeCover()
        {
            if (m_inCover || !m_PlayerRaycaster.CanTakeCover(out float angle, out _, out Vector3 point))
                return;

            m_CameraLogic.SwitchCameraSetting("coverLeft");

            m_inCover = true;
            m_Animator.SetBool(m_animIDInCover, InCover);

            Debug.LogWarning("Angle Value : " + angle);
            Vector3 direction = (transform.position - point).normalized;
            Vector3 coverPosition = point + .9f * m_CharacterController.radius * direction;

            transform.position = coverPosition;
            transform.rotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * (angle - 180));

            //transform.SetPositionAndRotation(coverPosition, Quaternion.Euler(transform.eulerAngles + Vector3.up * (angle - 180)));
        }

        public void LeaveCover()
        {
            if (!m_inCover) 
                return;

            m_inCover = false;
            m_Animator.SetBool(m_animIDInCover, m_inCover);
            m_CameraLogic.SwitchCameraSetting(m_Player.RightShoulder ? "rightStand" : "leftStand");
        }

        public void SetCharacterControllerHeightCenter()
        {
            if(m_isRolling || m_isCrouched)
            {
                m_CharacterController.height = m_crouchHeight;
                Vector3 center = m_CharacterController.center;
                m_CharacterController.center = new Vector3(center.x, m_crouchCenter, center.z);

            }
            else
            {
                m_CharacterController.height = m_standHeight;
                Vector3 center = m_CharacterController.center;
                m_CharacterController.center = new Vector3(center.x, m_standCenter, center.z);
            }
        }

        //public void Climb()
        //{
        //    m_Animator.SetTrigger(m_animIDClimb);
        //}

        public void DeactivateMovement()
        {
            m_canMove = false;
        }

        public void ActivateMovement()
        {
            m_canMove = true;
        }

        public void DeactivateJump()
        {
            m_canJump = false;
        }

        public void ActivateJump()
        {
            m_canJump = true;
        }

        private void Update()
        {
            Gravity();
            if (m_isSwimming) 
                ApplySwimForces();
            ManageRoll();
        }
    }
}