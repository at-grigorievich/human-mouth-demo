using UnityEngine;
using ATG.Input;
using ATG.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewRotateState : Statement
    {
        private readonly ITransformBehaviour _transformBehaviour;
        private readonly IInputService _inputService;

        private bool _isAllowRotate;

        public MouthViewRotateState(ITransformBehaviour transformBehaviour, IInputService inputService,
            IStateSwitcher sw) : base(sw)
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
            if(_isAllowRotate == false || _inputService.IsAllowMovement == true) return;

            _transformBehaviour.Move(Vector2.zero);
            _transformBehaviour.Rotate(_inputService.MouseAxis);
        }

        private void InputServiceEventHandler(InputEventType eventType)
        {
            switch (eventType)
            {
                case InputEventType.AllowRotate:
                    _isAllowRotate = true;
                    break;
                case InputEventType.DisallowRotate:
                    _isAllowRotate = false;
                    break;
            }
        }
    }
}