#nullable enable

using System;
using System.Collections.Generic;
using ATG.Input;
using ATG.Views;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewChooseToothState : Statement
    {
        public const int MaxChooseToothCount = 3;
        
        private readonly IInputService _inputService;

        private readonly HashSet<ToothView> _choosedTeeth;

        private readonly Func<ToothView?> _getSelectedTooth;

        public MouthViewChooseToothState(HashSet<ToothView> choosedTeeth, IInputService inputService,
             Func<ToothView?> getSelectedTooth, IStateSwitcher sw) : base(sw)
        {
            _inputService = inputService;

            _choosedTeeth = choosedTeeth;
            _getSelectedTooth = getSelectedTooth;
        }

        public override void Enter()
        {
            _inputService.OnInputEvent += InputServiceEventHandler;
        }

        public override void Exit()
        {
            _inputService.OnInputEvent -= InputServiceEventHandler;
        }

        public override void Execute() { }

        private void InputServiceEventHandler(InputEventType eventType)
        {
            switch(eventType)
            {
                case InputEventType.Choose:
                    ChooseTooth();
                    break;
                case InputEventType.Drag:
                    ChooseTooth(true);
                    SwitchToDrag();
                    break;
            }
        }

        private void ChooseTooth(bool igoreEquals = false)
        {   
            ToothView? lastSelectedTooth = _getSelectedTooth?.Invoke() ?? null;

            if(lastSelectedTooth == null) return;

            if(igoreEquals == false)
            {
                if(_choosedTeeth.Contains(lastSelectedTooth))
                {
                    lastSelectedTooth.Unchoose();
                    _choosedTeeth.Remove(lastSelectedTooth);
                    
                    return;
                }
            }
            
            if(_choosedTeeth.Count >= MaxChooseToothCount)
            {
                foreach(var tooth in _choosedTeeth)
                {
                    tooth.Unchoose();
                    tooth.Unselect();
                }
                _choosedTeeth.Clear();
            }
            
            lastSelectedTooth.Choose();
            _choosedTeeth.Add(lastSelectedTooth);
        }
        private void SwitchToDrag()
        {
            if(_choosedTeeth.Count == 0) return;
            _stateSwitcher.SwitchState<MouthViewDragTeethState>();
        }
    }
}