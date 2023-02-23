using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        private InputAsset m_Input;
        private Camera m_Camera;

        public override void Awake()
        {
            base.Awake();
            m_Input = GetComponent<InputAsset>();
            m_Camera = Camera.main;
        }

        private void Update()
        {
            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir, m_Camera.transform);

            if (m_Input.jump)
            {
                m_Movement.Jump();
                m_Input.jump = false;
            }

            if (m_Input.roll)
            {
                m_Movement.Roll();
                m_Input.roll = false;
            }
        }
    }
}