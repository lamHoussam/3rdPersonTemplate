using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem
{
    public class WaitAction : AIAction
    {
        [SerializeField] private float m_waitTime;
        private float m_timer;

        public override void InstantExecute()
        {
            base.InstantExecute();
            m_timer = 0;
        }

        private void Update()
        {
            if (!m_active) return;
            m_timer += Time.deltaTime;

            if (m_timer > m_waitTime)
                OnEnd();
        }
    }
}