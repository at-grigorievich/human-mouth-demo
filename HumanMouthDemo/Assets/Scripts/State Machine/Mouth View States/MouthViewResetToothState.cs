using System.Collections.Generic;
using ATG.Views;

using UnityTransform = UnityEngine.Transform;

namespace ATG.StateMachine.Views
{
    public sealed class MouthViewResetToothState : Statement
    {
        private readonly IEnumerable<ToothView> _allTeeth;
        private readonly UnityTransform _mouth;

        private int _resetTeethCount;

        public MouthViewResetToothState(IEnumerable<ToothView> allTeeth, UnityTransform mouth, IStateSwitcher sw) 
            : base(sw)
        {
            _allTeeth = allTeeth;
            _mouth = mouth;
        }

        public override void Enter()
        {
            foreach (var tooth in _allTeeth)
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