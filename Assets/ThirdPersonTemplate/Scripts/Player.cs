using CameraSystem;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        private InputAsset m_Input;
        private CameraController m_Camera;

        [SerializeField] private CameraSettings m_StandCameraSettings, m_CrouchCameraSettings;
        [SerializeField] private CameraSettings m_LeftCameraSettings, m_RightCameraSettings;

        private bool m_rightShoulder;


        public override void Awake()
        {
            base.Awake();

            m_Input = GetComponent<InputAsset>();
            m_Camera = Camera.main.GetComponent<CameraController>();

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

        }
    }
}