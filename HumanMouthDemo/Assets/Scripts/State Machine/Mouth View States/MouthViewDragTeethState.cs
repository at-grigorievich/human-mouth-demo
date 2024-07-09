using ATG.Input;
using ATG.Views;
using System;
using System.Collections.Generic;

using UnityTransform = UnityEngine.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewDragTeethState : Statement
    {
        private readonly IInputService _inputService;
        private readonly HashSet<ToothView> _choosedTeeth;

        private readonly UnityTransform _mouth;
        private readonly UnityTransform _dragParent;

        private readonly Action<ToothView> _setSelectTooth;

        public MouthViewDragTeethState(IInputService inputService, HashSet<ToothView> choosedTeed,
            UnityTransform mouth, UnityTransform dragParent, Action<ToothView> setSelectTooth, IStateSwitcher sw)
            : base(sw)
        {
            _inputService = inputService;
            _choosedTeeth = choosedTeed;

            _mouth = mouth;
            _dragParent = dragParent;

            _setSelectTooth = setSelectTooth;
        }

        public override void Enter()
        {
            _inputService.OnInputEvent += InputServiceEventHandler;

            foreach (var tooth in _choosedTeeth)
            {
                tooth.SetParent(_dragParent);
                
                tooth.StartAnimate();
            }
        }

        public override void Exit()
        {
            _inputService.OnInputEvent -= InputServiceEventHandler;

            foreach (var tooth in _choosedTeeth)
            {
                tooth.StopAnimate();

                tooth.SetParent(_mouth);
                tooth.Unchoose();
                tooth.Unselect();
            }

            _choosedTeeth.Clear();

            _setSelectTooth?.Invoke(null);
        }

        public override void Execute()
        { }

        private void InputServiceEventHandler(InputEventType eventType)
        {
            switch (eventType)
            {
                case InputEventType.Choose:
                    _stateSwitcher.SwitchState<MouthViewChooseToothState>();
                    break;
            }
        }
    }
}