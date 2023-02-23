using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AISystem
{
    public abstract class AIElement : MonoBehaviour
    {
        [SerializeField] protected GameObject m_Target;
        [SerializeField] private AIElement m_Next;
        public AIElement Next => m_Next;

        protected bool m_active = false;

        public virtual void InstantExecute()
        {
            m_active = true;
        }

        public virtual void OnEnd()
        {
            m_active = false;
        }
    }

}
