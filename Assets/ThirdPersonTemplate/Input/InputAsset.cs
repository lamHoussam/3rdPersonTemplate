using UnityEditor.iOS.Xcode;
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
        public bool roll;
        public bool crouch;
        public bool sprint;
        public bool switchShoulder;
        public bool cover;
        public bool aim;
        public bool fire;

        public bool interact;

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

        private void OnRoll(InputValue value)
        {
            roll = value.isPressed;
        }

        private void OnCrouch(InputValue value)
        {
            crouch = value.isPressed;
        }

        private void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

        private void OnSwitch(InputValue value)
        {
            switchShoulder = value.isPressed;
        }

        private void OnCover(InputValue value)
        {
            cover = value.isPressed;
        }

        private void OnAim(InputValue value)
        {
            aim = value.isPressed;
        }

        private void OnFire(InputValue value)
        {
            fire = value.isPressed;
        }

        private void OnInteract(InputValue value)
        {
            interact = value.isPressed;
        }
    }
}