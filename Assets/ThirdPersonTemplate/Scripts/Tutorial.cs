using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Tutorial : MonoBehaviour
    {
        private bool m_active;
        public bool Active => m_active;

        public void Activate()
        {
            m_CurrentInstruction = m_HeadInstruction;
            m_TutorialText.text = m_CurrentInstruction.Desctiption;

            CurrentInstruction.OnStartInstruction();
            m_active = true;
        }

        [SerializeField] private TextMeshProUGUI m_TutorialText;
        [SerializeField] private TutorialInstruction m_HeadInstruction;

        private TutorialInstruction m_CurrentInstruction;
        public TutorialInstruction CurrentInstruction => m_CurrentInstruction;

        private void Awake()
        {
            m_active = false;
        }

        private void Start()
        {
            //Activate();
        }

        public void MoveToNextInstruction()
        {
            m_CurrentInstruction = CurrentInstruction.NextInstruction;
            if (m_CurrentInstruction == null)
            {
                m_TutorialText.text = "";
                m_active = false;
                Destroy(gameObject);
            }
            else
            {
                CurrentInstruction.OnStartInstruction();
                m_TutorialText.text = m_CurrentInstruction.Desctiption;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Player player))
            {
                Activate();
            }
        }
    }
}