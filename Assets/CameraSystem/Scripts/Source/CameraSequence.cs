using UnityEngine;

public class CameraSequence : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;
    [SerializeField] private Transform[] m_CameraTargets;
    [SerializeField] private float m_lerpTime;
    [SerializeField] private bool m_loop;

    [SerializeField] private AnimationCurve m_SequenceCurve;

    private bool m_sequenceStarted;
    private int m_currentCameraTargetIndex;

    private Vector3 direction;

    private float t;

    private void Start()
    {
        //StartSequence();
    }

    /// <summary>
    /// Start sequence
    /// </summary>
    public void StartSequence()
    {
        m_currentCameraTargetIndex = 0;
        m_sequenceStarted = true;

        m_Camera.SetPositionAndRotation(m_CameraTargets[0].position, m_CameraTargets[0].rotation);

        m_currentCameraTargetIndex++;
        direction = (m_CameraTargets[m_currentCameraTargetIndex].position - m_Camera.position).normalized;
        t = 0;
    }


    /// <summary>
    /// Stop sequence
    /// </summary>
    public void StopSequence() => m_sequenceStarted = false; 


    public void Update()
    {
        if (!m_sequenceStarted) 
            return;

        Vector3 targetPos = m_CameraTargets[m_currentCameraTargetIndex].position;
        Quaternion targetRotation = m_CameraTargets[m_currentCameraTargetIndex].rotation;

        t += Time.deltaTime;
        Vector3 pos = m_Camera.position + m_SequenceCurve.Evaluate(t) * Time.deltaTime * direction;

        m_Camera.SetPositionAndRotation(
            pos,
            targetRotation
        );


        float distance = Vector3.Distance(m_Camera.position, targetPos);
        if (distance < .1f)
        {
            m_currentCameraTargetIndex++;
            if (m_currentCameraTargetIndex >= m_CameraTargets.Length)
                StopSequence();
            else
                direction = (m_CameraTargets[m_currentCameraTargetIndex].position - m_Camera.position).normalized;
            //t = 0;
        }

    }
}
