using CameraSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        private InputAsset m_Input;
        private CameraController m_Camera;
        private PlayerRaycaster m_PlayerRaycaster;

        [SerializeField] private CameraSettings m_StandCameraSettings, m_CrouchCameraSettings;
        [SerializeField] private CameraSettings m_LeftCameraSettings, m_RightCameraSettings;
        [SerializeField] private CameraSettings m_AimCameraSettings;

        private bool m_rightShoulder;
        private bool m_isAiming;

        public UnityEvent m_OnMove, m_OnJump, m_OnCrouch;

        private IInteractable m_NearInteractable;

        public override void Awake()
        {
            base.Awake();

            m_Input = GetComponent<InputAsset>();
            m_Camera = Camera.main.GetComponent<CameraController>();
            m_PlayerRaycaster = GetComponent<PlayerRaycaster>();

            m_rightShoulder = true;

        }

        private void Update()
        {
            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir, m_Input.sprint, m_Camera.transform);
            m_Camera.SetPitchYaw(m_Input.look);


            if (m_Input.jump)
            {
                m_Movement.Jump();

                m_Input.jump = false;
            }

            if (m_Input.roll)
            {
                m_Movement.Roll(moveDir, m_Camera.transform);
                m_Input.roll = false;
            }

            if(m_Input.crouch)
            {
                m_Movement.ChangeCrouchStandState();
                m_Camera.BlendBetweenCameraSettings(m_Movement.IsCrouched ? m_CrouchCameraSettings : m_StandCameraSettings);

                m_Input.crouch = false;
            }

            if (m_Input.switchShoulder)
            {
                m_rightShoulder = !m_rightShoulder;
                m_Camera.BlendBetweenCameraSettings(m_rightShoulder ? m_RightCameraSettings : m_LeftCameraSettings);

                m_Input.switchShoulder = false;
            }

            if (m_Input.cover)
            {
                if (m_Movement.InCover)
                    m_Movement.LeaveCover();
                else
                    m_Movement.TakeCover();

                m_Input.cover = false;
            }

            if (m_Input.aim)
            {
                m_isAiming = !m_isAiming;

                m_Input.aim = false;
                m_Camera.BlendBetweenCameraSettings(m_isAiming ? m_AimCameraSettings : m_StandCameraSettings);
            }

            if (m_Input.fire)
            {
                m_PlayerRaycaster.Fire();
                m_Input.fire = false;
            }

            if(m_NearInteractable != null && m_Input.interact)
            {
                m_NearInteractable.OnInteract(this);
                m_Input.interact = false;
            }

            //if (m_isAiming)
            //{

            //}

        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IInteractable interactable))
            {
                m_NearInteractable = interactable;
                m_NearInteractable.OnBeginOverlap(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable _))
            {
                m_NearInteractable.OnEndOverlap(this);
                m_NearInteractable = null;
            }
        }
    }
}