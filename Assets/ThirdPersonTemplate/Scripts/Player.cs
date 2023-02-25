using CameraSystem;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        private InputAsset m_Input;
        private CameraController m_Camera;

        [SerializeField] private CameraSettings m_StandCameraSettings, m_CrouchCameraSettings;

        public override void Awake()
        {
            base.Awake();

            m_Input = GetComponent<InputAsset>();
            m_Camera = Camera.main.GetComponent<CameraController>();
        }

        private void Update()
        {
            if (m_Input.jump)
            {
                m_Movement.Jump();
                m_Input.jump = false;
            }

            if (m_Input.roll)
            {
                m_Movement.Roll(m_Camera.transform.forward);
                m_Input.roll = false;
            }

            if(m_Input.crouch)
            {
                m_Movement.Crouch();

                m_Camera.BlendBetweenCameraSettings(m_Movement.IsCrouched ? m_CrouchCameraSettings : m_StandCameraSettings);
                m_Input.crouch = false;
            }


            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir, m_Camera.transform);

        }
    }
}