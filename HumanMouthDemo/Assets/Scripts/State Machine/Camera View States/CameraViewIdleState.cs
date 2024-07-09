using ATG.Input;
using UnityEngine;

namespace ATG.StateMachine.Views
{
    public sealed class CameraViewIdleState : Statement
    {
        private readonly IInputService _inputService;

        public CameraViewIdleState(IInputService inputService, IStateSwitcher sw): base(sw)
        {
            _inputService = inputService;
        }

        public override void Enter()
        {
            Cursor.lockState = CursorLockMode.None;
            _inputService.OnInputEvent += InputServiceEventHandler;
        }

        public override void Exit()
        {
            _inputService.OnInputEvent -= InputServiceEventHandler;
        }

        public override void Execute(){}

        private void InputServiceEventHandler(InputEventType eventType)
        {
            switch(eventType)
            {
                case InputEventType.AllowMovement:
                    _stateSwitcher.SwitchState<CameraViewMoveState>();
                    break;
            }
        }
    }
}