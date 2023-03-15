using CameraSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        protected InputAsset m_Input;

        protected CameraController m_CameraController;
        protected CameraLogicGraph m_CameraGraph;

        protected PlayerRaycaster m_PlayerRaycaster;

        protected bool m_rightShoulder;
        public bool RightShoulder => m_rightShoulder;

        public UnityEvent m_OnMove, m_OnJump, m_OnCrouch;

        protected IInteractable m_NearInteractable;

        public void InitCameraSettings()
        {
            m_CameraGraph.SetBool("crouch", false, false);
            m_CameraGraph.SetBool("cover", false, false);
            m_CameraGraph.SetBool("rightShoulder", m_rightShoulder);

        }

        public override void Awake()
        {
            base.Awake();

            m_Input = GetComponent<InputAsset>();

            m_CameraController = Camera.main.GetComponent<CameraController>();
            m_CameraGraph = m_CameraController.GetComponent<CameraLogicGraph>();

            m_PlayerRaycaster = GetComponent<PlayerRaycaster>();

            m_rightShoulder = true;
            InitCameraSettings();

        }

        public virtual void Update()
        {
            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir, m_Input.sprint, m_CameraController.transform);
            m_CameraController.SetPitchYaw(m_Input.look);


            if (m_Input.jump)
            {
                m_Movement.Jump();

                m_Input.jump = false;
            }

            if (m_Input.roll)
            {
                m_Movement.Roll(moveDir, m_CameraController.transform);
                m_Input.roll = false;
            }

            if(m_Input.crouch)
            {
                m_Movement.ChangeCrouchStandState();

                m_Input.crouch = false;
            }

            if (m_Input.switchShoulder && !m_Movement.InCover)
            {
                SwitchShoulders();
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

            if(m_NearInteractable != null && m_Input.interact)
            {
                m_NearInteractable.OnInteract(this);
                m_Input.interact = false;
            }

        }

        public virtual void SwitchShoulders()
        {
            //if (m_Movement.InCover)
            //    return;

            m_rightShoulder = !m_rightShoulder;

            //m_CameraController.GetComponent<CameraLogic>().SwitchCameraSetting(m_rightShoulder ? "rightStand" : "leftStand");
            m_CameraGraph.SetBool("rightShoulder", m_rightShoulder);
            //Debug.Log("Right Shoulder value in graph : " + m_CameraGraph.GetBool("rightShoulder") + ";<()>;RS value : " + m_rightShoulder);
            //Debug.LogWarning("Crouch value in Graph : " + m_CameraGraph.GetBool("crouch") + ";<()>;" + m_Movement.IsCrouched);
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