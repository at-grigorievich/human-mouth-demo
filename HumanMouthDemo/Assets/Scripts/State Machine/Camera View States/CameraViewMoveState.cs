using System.Diagnostics;
using System.Threading;
using ATG.Input;
using ATG.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class CameraViewMoveState : Statement
    {
        private readonly ITransformBehaviour _transformBehaviour;
        private readonly IInputService _inputService;

        public CameraViewMoveState(ITransformBehaviour transformBehaviour, 
                IInputService inputService, IStateSwitcher sw): base(sw)
        {
            _transformBehaviour = transformBehaviour;
            _inputService = inputService;
        }

        public override void Enter()
        {
            _inputService.OnInputEvent += InputServiceEventHandler;
        }

        public override void Exit()
        {
            _inputService.OnInputEvent -= InputServiceEventHandler;
        }

        public override void Execute()
        {
            _transformBehaviour.Move(_inputService.KeyboardAxis);
            _transformBehaviour.Rotate(_inputService.MouseAxis);
        }

        private void InputServiceEventHandler(InputEventType eventType)
        {
            switch(eventType)
            {
                case InputEventType.DisallowMovement:
                    _stateSwitcher.SwitchState<CameraViewIdleState>();
                    break;
            }
        }
    }
}