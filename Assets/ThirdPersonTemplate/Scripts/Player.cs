using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Player : MonoBehaviour
    {
        private Movement m_Movement;
        private InputAsset m_Input;

        private void Awake()
        {
            m_Movement = GetComponent<Movement>();
            m_Input = GetComponent<InputAsset>();
        }

        private void Update()
        {
            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir);
        }
    }
}