using System.Collections.Generic;
using UnityEngine;

namespace Sample.Components
{
    public class InputComponent : Component
    {
        private static readonly Dictionary<KeyBinding, KeyCode> _keyBindings = new()
        {
            {KeyBinding.Action0, KeyCode.Return},
            {KeyBinding.Jump, KeyCode.Space},
            {KeyBinding.Dash, KeyCode.LeftShift},
            {KeyBinding.Left, KeyCode.A},
            {KeyBinding.Right, KeyCode.D},
            {KeyBinding.Up, KeyCode.W},
            {KeyBinding.Down, KeyCode.S},
        };

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
            _state.Action0Pressed = Input.GetKeyDown(_keyBindings[KeyBinding.Action0]);
            _state.Action0Released = Input.GetKeyUp(_keyBindings[KeyBinding.Action0]);

            _state.JumpPressed = Input.GetKeyDown(_keyBindings[KeyBinding.Jump]);
            _state.JumpReleased = Input.GetKeyUp(_keyBindings[KeyBinding.Jump]);

            _state.DashPressed = Input.GetKeyDown(_keyBindings[KeyBinding.Dash]);
            _state.DashPressed = Input.GetKeyUp(_keyBindings[KeyBinding.Dash]);

            _state.MoveUp = Input.GetKey(_keyBindings[KeyBinding.Up]);
            _state.MoveDown = Input.GetKey(_keyBindings[KeyBinding.Down]);
            _state.MoveLeft = Input.GetKey(_keyBindings[KeyBinding.Left]);
            _state.MoveRight = Input.GetKey(_keyBindings[KeyBinding.Right]);

            _state.Pointer = Input.mousePosition;
        }
    }

    public enum KeyBinding
    {
        Action0,
        Jump,
        Dash,
        Left,
        Right,
        Up,
        Down,
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
