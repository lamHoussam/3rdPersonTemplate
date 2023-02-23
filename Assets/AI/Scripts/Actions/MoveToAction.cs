using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AISystem
{
    public class MoveToAction : AIAction
    {
        [SerializeField] private Transform m_MoveToTarget;

        private NavMeshAgent m_NavMesh;

        public override void InstantExecute()
        {
            base.InstantExecute();
            m_NavMesh = m_Target.GetComponent<NavMeshAgent>();
            m_NavMesh.SetDestination(m_MoveToTarget.position);

            m_Target.transform.LookAt(m_MoveToTarget);
        }

        private void Update()
        {
            if (!m_active) return;

            float distance = Vector3.Distance(m_MoveToTarget.position, m_Target.transform.position);
            if (distance < .1f)
                OnEnd();
        }
    }
}