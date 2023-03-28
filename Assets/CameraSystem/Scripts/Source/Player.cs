    using UnityEngine;

namespace CameraSystem
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float RotationSmoothTime;
        [Space]
        [SerializeField] private CameraSettings m_StandSettings;
        [SerializeField] private CameraSettings m_CrouchSettings;

        private bool m_crouched;

        private CameraLogicGraph m_CameraLogic;
        private float m_targetRotation;

        private bool m_rightShoulder;

        private void Awake()
        {
            m_crouched = false;
            m_rightShoulder = false;


            m_CameraLogic = Camera.main.GetComponent<CameraLogicGraph>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;


            if (direction != Vector3.zero)
            {
                float rotationVelocity = 0;
                m_targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                  m_CameraLogic.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                Vector3 targetDirection = Quaternion.Euler(0.0f, m_targetRotation, 0.0f) * Vector3.forward;
                transform.position += m_speed * Time.deltaTime * targetDirection.normalized;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                m_crouched = !m_crouched;
                Debug.Log(m_crouched);
                m_CameraLogic.SetBool("crouch", m_crouched);

                //m_CameraLogic.SwitchCameraSetting(m_crouched ? "crouch" : "stand");
                //m_CameraLogic.GetComponent<CameraController>().BlendBetweenCameraSettings(m_crouched ? m_CrouchSettings : m_StandSettings);
            }

            if (Input.GetKeyDown(KeyCode.Q))
                SwitchShoulder();
        }
        public void SwitchShoulder()
        {
            Debug.Log("Switch");
            m_rightShoulder = !m_rightShoulder;
            m_CameraLogic.SetBool("rightShoulder", m_rightShoulder);
        }
    }
}