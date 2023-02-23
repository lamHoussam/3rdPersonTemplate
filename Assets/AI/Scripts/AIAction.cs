using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem
{
    public class AIAction : AIElement
    {
        //[SerializeField] private AIElement m_NextElement;

        public override void InstantExecute()
        {
            base.InstantExecute();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Next?.InstantExecute();
        }
    }
}