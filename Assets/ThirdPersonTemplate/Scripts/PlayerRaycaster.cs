using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] private Transform m_CanStandRaycastOrigin;
        [SerializeField] private float m_CanStandDistance;

        [Space]

        [SerializeField] private Transform m_CoverRaycastOrigin;
        [SerializeField] private float m_CanTakeCoverDistance;
        [SerializeField] private LayerMask m_CoverLayer;

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


    }
}