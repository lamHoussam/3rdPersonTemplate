using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] private Transform m_CanStandRaycastOrigin;
        [SerializeField] private float m_CanStandDistance;

        public bool CanStand()
        {
            Ray ray = new Ray(m_CanStandRaycastOrigin.position, transform.up);

            Debug.DrawRay(ray.origin, ray.direction * m_CanStandDistance, Color.red, 10);
            return !Physics.Raycast(ray, m_CanStandDistance);
        }

    }
}