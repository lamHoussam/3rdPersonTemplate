using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class TutorialInstruction : MonoBehaviour
    {
        [SerializeField] private string m_description;
        public string Desctiption => m_description;

        [SerializeField] private TutorialInstruction m_NextInstruction;
        public TutorialInstruction NextInstruction => m_NextInstruction;

        public virtual void OnCompleteInstruction()
        {
            GetComponentInParent<Tutorial>().MoveToNextInstruction();
        }

        public virtual void OnStartInstruction()
        {

        }
    }
}