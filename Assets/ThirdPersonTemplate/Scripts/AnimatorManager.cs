using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ThirdPersonTemplate
{
    public class AnimatorManager : MonoBehaviour
    {

        private Movement m_Movement;

        private void Awake()
        {
            m_Movement = GetComponentInParent<Movement>();
        }

        public void OnStopRoll() => m_Movement.OnStopRoll();
    }
}