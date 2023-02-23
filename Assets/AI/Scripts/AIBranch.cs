using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem
{
    public class AIBranch : AIElement
    {

        [SerializeField] private AIElement m_ElseNextElement;
        public override void InstantExecute()
        {
            base.InstantExecute();
            OnEnd();
        }

        public override void OnEnd()
        {
            base.OnEnd();

            if (CheckCondition())
                Next.InstantExecute();
            else
                m_ElseNextElement.InstantExecute();

        }
        public bool CheckCondition()
        {
            return false;
        }
    }
}