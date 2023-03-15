using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace ThirdPersonTemplate
{
    [CustomEditor(typeof(ThirdPersonTemplate.Player))]
    public class PlayerEditor : Editor
    {
        private void OnEnable()
        {
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}

#endif
