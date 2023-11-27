using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    public class KeyBindings
    {
        private static readonly Dictionary<InputKey, KeyCode> _keyBindings = new()
        {
            {InputKey.Attack, KeyCode.Return},
            {InputKey.Jump, KeyCode.Space},
            {InputKey.Dash, KeyCode.LeftShift},
            {InputKey.Left, KeyCode.A},
            {InputKey.Right, KeyCode.D},
            {InputKey.Up, KeyCode.W},
            {InputKey.Down, KeyCode.S},
            {InputKey.Menu, KeyCode.Escape},
        };

        public static bool IsKeyDown(InputKey inputKey)
        {
            return Input.GetKey(_keyBindings[inputKey]);
        }

        public static bool IsKeyPressed(InputKey inputKey)
        {
            return Input.GetKeyDown(_keyBindings[inputKey]);
        }

        public static bool IsKeyReleased(InputKey inputKey)
        {
            return Input.GetKeyUp(_keyBindings[inputKey]);
        }
    }

    public enum InputKey
    {
        Attack,
        Jump,
        Dash,
        Left,
        Right,
        Up,
        Down,
        Menu,
    }
}
