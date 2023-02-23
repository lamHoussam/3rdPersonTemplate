using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Humanoid : MonoBehaviour
    {
        protected Movement m_Movement;

        public virtual void Awake()
        {
            m_Movement = GetComponent<Movement>();
        }

    }
}