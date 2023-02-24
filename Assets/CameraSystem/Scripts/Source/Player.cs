using CameraSystem;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float RotationSmoothTime;
    [Space]
    [SerializeField] private CameraSettings m_StandSettings;
    [SerializeField] private CameraSettings m_CrouchSettings;

    private bool m_crouched;

    private Camera m_Camera;
    private float m_targetRotation;
    private void Awake()
    {
        m_crouched = false;
        m_Camera = Camera.main;
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
                              m_Camera.transform.eulerAngles.y;
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
            m_Camera.GetComponent<CameraController>().BlendBetweenCameraSettings(m_crouched ? m_CrouchSettings : m_StandSettings);
        }


        // move the player

    }    
}
