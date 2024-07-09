using System;
using UnityEngine;

namespace ATG.Input
{
    public enum InputEventType: byte
    {
        None = 0,
        AllowMovement = 1,
        DisallowMovement = 2
    }

    public interface IInputService
    {
        public Vector2 KeyboardAxis {get;}
        public Vector2 MouseAxis {get;}

        public event Action<InputEventType> OnInputEvent;
    }
}
