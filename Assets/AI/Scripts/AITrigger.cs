using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AISystem
{
    public class AITrigger : AIElement
    {
        [SerializeField] private AIElement m_OnTrueElement, m_OnFalseElement;

        public bool Condition()
        {
            return false;
        }

        public override void InstantExecute()
        {
            base.InstantExecute();
        }
    }

}
