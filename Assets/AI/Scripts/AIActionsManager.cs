using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISystem
{
    public class AIActionsManager : MonoBehaviour
    {
        [SerializeField] private AIElement m_ActionsGraphHead;
        public AIElement ActionsGraphHead => m_ActionsGraphHead;

        public void StartActionsGraph()
        {
            m_ActionsGraphHead?.InstantExecute();
        }
    }
}