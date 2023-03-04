using Unity.VisualScripting;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class PlayerRaycaster : MonoBehaviour
    {
        // Can stand Raycasts values
        [SerializeField] private Transform m_CanStandRaycastOrigin;
        [SerializeField] private float m_CanStandDistance;


        // Cover Raycasts values
        [Space]
        [SerializeField] private Transform m_CoverRaycastOrigin;
        [SerializeField] private float m_CanTakeCoverDistance;
        [SerializeField] private LayerMask m_CoverLayer;

        [Space]
        [SerializeField] private Transform m_Eyes;
        [SerializeField] private float m_FireDistance;

        [Space]
        [SerializeField] private Transform m_ClimbRaycastOrigin;
        [SerializeField] private float m_CanClimbDistance;
        [SerializeField] private LayerMask m_ClimbLayer; 

        public bool CanStand()
        {
            Ray ray = new Ray(m_CanStandRaycastOrigin.position, transform.up);

            Debug.DrawRay(ray.origin, ray.direction * m_CanStandDistance, Color.red, 10);
            return !Physics.Raycast(ray, m_CanStandDistance);
        }

        public bool CanTakeCover(out float ang, out Transform hitTransform, out Vector3 contactPoint)
        {
            Ray ray = new Ray(m_CoverRaycastOrigin.position, transform.forward);
            hitTransform = null;
            contactPoint = Vector3.zero;
            ang = 0;

            Debug.DrawRay(ray.origin, ray.direction * m_CanTakeCoverDistance, Color.green, 10);

            if (Physics.Raycast(ray, out RaycastHit hit, m_CanTakeCoverDistance, m_CoverLayer))
            {
                ang = Vector3.SignedAngle(m_CoverRaycastOrigin.position - hit.point, hit.normal, Vector3.up);
                hitTransform = hit.transform;
                contactPoint = hit.point;

                return true;
            }

            return false;
        }

        // TODO: Optimise
        public void Fire()
        {
            Vector2 centerPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(centerPoint);
            Vector3 point = ray.origin + ray.direction * m_FireDistance;
            Ray fireRay = new Ray(m_Eyes.position, (point - m_Eyes.position).normalized);

            if(Physics.Raycast(fireRay, out RaycastHit hit, m_FireDistance))
            {
                Vector3 dir = (hit.point - m_Eyes.position).normalized;

                Debug.Log("Change direction");
                Debug.DrawRay(fireRay.origin, dir * m_FireDistance, Color.green, 20);
            }

        }


        public bool CanClimb()
        {
            Ray ray = new Ray(m_ClimbRaycastOrigin.position, transform.forward);

            Debug.DrawRay(ray.origin, ray.direction * m_CanClimbDistance, Color.blue, 10);
            return Physics.Raycast(ray, m_CanClimbDistance, m_ClimbLayer);
        }
    }
}