using ATG.Input;
using ATG.Views;

using UnityTransform = UnityEngine.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewDragTeethState : Statement
    {
        private readonly IInputService _inputService;
        private readonly TeethSet _set;

        private readonly UnityTransform _dragParent;


        public MouthViewDragTeethState(IInputService inputService, TeethSet set,UnityTransform dragParent, 
            IStateSwitcher sw) : base(sw)
        {
            _inputService = inputService;
            _set = set;

            _dragParent = dragParent;
        }

        public override void Enter()
        {
            _inputService.OnInputEvent += InputServiceEventHandler;

            foreach (var tooth in _set.GetChoosedEnumerable())
            {
                tooth.SetParent(_dragParent);

                tooth.StartAnimate();
            }
        }

        public override void Exit()
        {
            _inputService.OnInputEvent -= InputServiceEventHandler;

            foreach (var tooth in _set.GetChoosedEnumerable())
            {
                tooth.StopAnimate();

                tooth.SetParent(null);
                tooth.Unchoose();
                tooth.Unselect();
            }

            _set.ChoosedClear();

            _set.ClearSelectedTooth();
        }

        public override void Execute() { }

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