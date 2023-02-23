using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Player : Humanoid
    {
        private InputAsset m_Input;

        public override void Awake()
        {
            base.Awake();
            m_Input = GetComponent<InputAsset>();
        }

        private void Update()
        {
            Vector3 moveDir = new Vector3(m_Input.move.x, 0, m_Input.move.y).normalized;
            m_Movement.Move(moveDir);

            if (m_Input.jump)
            {
                m_Movement.Jump();
                m_Input.jump = false;
            }
        }
    }
}