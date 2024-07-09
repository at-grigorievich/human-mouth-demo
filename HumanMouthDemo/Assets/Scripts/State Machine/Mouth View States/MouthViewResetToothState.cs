using ATG.Views;

using UnityTransform = UnityEngine.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewResetToothState : Statement
    {
        private readonly TeethSet _set;
        private readonly UnityTransform _mouth;

        private int _resetTeethCount;

        public MouthViewResetToothState(TeethSet set, UnityTransform mouth, IStateSwitcher sw): base(sw)
        {
            _set = set;
            _mouth = mouth;
        }

        public override void Enter()
        {
            foreach (var tooth in _set.GetEnumerable())
            {
                tooth.SetParent(_mouth);
                tooth.Reset(OnToothReset);
            }
        }

        public override void Execute() {}

        private void OnToothReset()
        {
            _resetTeethCount--;

            if(_resetTeethCount <= 0)
            {
                _stateSwitcher.SwitchState<MouthViewChooseToothState>();
            }
        }
    }
}