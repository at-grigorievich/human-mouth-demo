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

        private readonly TeethSet _set;

        public MouthViewChooseToothState(TeethSet set, IInputService inputService, IStateSwitcher sw) : base(sw)
        {
            _inputService = inputService;

            _set = set;
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
            switch (eventType)
            {
                case InputEventType.Choose:
                    ChooseTooth();
                    break;
                case InputEventType.Drag:
                    if (_set.LastSelectedTooth != null)
                    {
                        if (_set.ChoosedContains(_set.LastSelectedTooth) == false)
                        {
                            ChooseTooth();
                        }
                    }
                    SwitchToDrag();
                    break;
            }
        }

        private void ChooseTooth()
        {
            ToothView? lastSelectedTooth = _set.LastSelectedTooth;

            if (lastSelectedTooth == null) return;

            if (_set.ChoosedContains(lastSelectedTooth) == true)
            {
                lastSelectedTooth.Unchoose();
                _set.ChoosedRemove(lastSelectedTooth);

                return;
            }


            if (_set.ChoosedCount >= MaxChooseToothCount)
            {
                foreach (var tooth in _set.GetChoosedEnumerable())
                {
                    tooth.Unchoose();
                    tooth.Unselect();
                }
                _set.ChoosedClear();
            }

            lastSelectedTooth.Choose();
            _set.ChoosedAdd(lastSelectedTooth);
        }
        private void SwitchToDrag()
        {
            if (_set.ChoosedCount == 0) return;
            _stateSwitcher.SwitchState<MouthViewDragTeethState>();
        }
    }
}