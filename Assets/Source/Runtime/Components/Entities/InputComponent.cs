using UnityEngine;

namespace Sample.Components.Entities
{
    public class InputComponent : Component
    {
        private InputState _state;
        private Vector2 _pointer;

        public InputState GetState()
        {
            return _state;
        }

        public void ResetState()
        {
            _state = new InputState();
        }

        private void Update()
        {
            _state.Action0Pressed = KeyBindings.IsKeyPressed(InputKey.Attack);
            _state.Action0Released = KeyBindings.IsKeyReleased(InputKey.Attack);

            _state.JumpPressed = KeyBindings.IsKeyPressed(InputKey.Jump);
            _state.JumpReleased = KeyBindings.IsKeyReleased(InputKey.Jump);

            _state.DashPressed = KeyBindings.IsKeyPressed(InputKey.Dash);
            _state.DashPressed = KeyBindings.IsKeyReleased(InputKey.Dash);

            _state.MoveUp = KeyBindings.IsKeyDown(InputKey.Up);
            _state.MoveDown = KeyBindings.IsKeyDown(InputKey.Down);
            _state.MoveLeft = KeyBindings.IsKeyDown(InputKey.Left);
            _state.MoveRight = KeyBindings.IsKeyDown(InputKey.Right);

            _state.Pointer = Input.mousePosition;
        }
    }

    public struct InputState
    {
        public bool IsUsingGamepad;
        public Vector2 Pointer;
        public Vector2 RightStickVector;
        public float RightStickAngle;
        public Vector2 LeftStickVector;
        public float LeftStickAngle;
        public bool MoveLeft;
        public bool MoveRight;
        public bool MoveUp;
        public bool MoveDown;
        public bool Crouch;
        public bool DashPressed;
        public bool JumpPressed;
        public bool JumpReleased;
        public bool Action0Pressed;
        public bool Action1Pressed;
        public bool Action2Pressed;
        public bool Action3Pressed;
        public bool Action4Pressed;
        public bool Action5Pressed;
        public bool Action0Released;
        public bool Action1Released;
        public bool Action2Released;
        public bool Action3Released;
        public bool Action4Released;
        public bool Action5Released;

        public void CleanupTriggers()
        {
            DashPressed = false;

            JumpPressed = false;
            JumpReleased = false;

            Action0Pressed = false;
            Action1Pressed = false;
            Action2Pressed = false;
            Action3Pressed = false;
            Action4Pressed = false;
            Action5Pressed = false;

            Action0Released = false;
            Action1Released = false;
            Action2Released = false;
            Action3Released = false;
            Action4Released = false;
            Action5Released = false;
        }
    }
}
