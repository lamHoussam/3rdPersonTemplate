using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonTemplate
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputAsset : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;

        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        private void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        private void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }
    }
}