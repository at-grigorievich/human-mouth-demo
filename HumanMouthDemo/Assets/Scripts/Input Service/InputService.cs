using System;
using ATG.Activation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATG.Input
{
    public sealed class InputService : ActivateObject, IInputService
    {
        private readonly InputControl _inputControl;

        public Vector2 KeyboardAxis => _inputControl?.Input.Move.ReadValue<Vector2>() ?? Vector2.zero;
        public Vector2 MouseAxis => _inputControl?.Input.Rotate.ReadValue<Vector2>() ?? Vector2.zero;

        public bool IsAllowMovement { get; private set; }

        public event Action<InputEventType> OnInputEvent;

        public InputService(InputControl inputControl)
        {
            _inputControl = inputControl;
        }

        public override void SetActive(bool isActive)
        {
            if (isActive)
            {
                EnableInputControl();
            }
            else
            {
                DisableInputControl();
            }

            base.SetActive(isActive);
        }

        private void EnableInputControl()
        {
            _inputControl.Enable();

            _inputControl.Input.AllowMovement.performed += OnAllowMovement;
            _inputControl.Input.DisallowMovement.performed += OnDisallowMovement;
            _inputControl.Input.Choose.performed += OnChoose;
            _inputControl.Input.Drag.performed += OnDrag;
            _inputControl.Input.AllowRotate.performed += OnAllowRotate;
            _inputControl.Input.DisallowRotate.performed += OnDisallowRotate;
        }

        private void DisableInputControl()
        {
            _inputControl.Disable();

            _inputControl.Input.AllowMovement.performed -= OnAllowMovement;
            _inputControl.Input.DisallowMovement.performed -= OnDisallowMovement;
            _inputControl.Input.Choose.performed -= OnChoose;
            _inputControl.Input.Drag.performed -= OnDrag;
            _inputControl.Input.AllowRotate.performed -= OnAllowRotate;
            _inputControl.Input.DisallowRotate.performed -= OnDisallowRotate;

            IsAllowMovement = false;
        }

        private void OnAllowMovement(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.AllowMovement);

            IsAllowMovement = true;
        }

        private void OnDisallowMovement(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.DisallowMovement);

            IsAllowMovement = false;
        }

        private void OnChoose(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.Choose);
        }

        private void OnDrag(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.Drag);
        }

        private void OnAllowRotate(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.AllowRotate);
        }

        private void OnDisallowRotate(InputAction.CallbackContext _)
        {
            OnInputEvent?.Invoke(InputEventType.DisallowRotate);
        }
    }
}
