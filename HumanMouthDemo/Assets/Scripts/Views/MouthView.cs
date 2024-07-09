using System;
using System.Collections.Generic;
using ATG.Activation;
using ATG.Input;
using ATG.StateMachine.Views;
using UnityEngine;

using UnityTransform = UnityEngine.Transform;
using SM = ATG.StateMachine.StateMachine;

namespace ATG.Views
{
    [Serializable]
    public sealed class MouthViewFactory
    {
        [SerializeField] private UnityTransform mouthTransform;
        [SerializeField] private UnityTransform dragParentTransform;
        [Space(5)]
        [SerializeField] private GameObject[] teethes;

        public MouthView Create(IInputService inputService)
        {
            Dictionary<int, ToothView> data = new();

            for (int i = 0; i < teethes.Length; i++)
            {
                GameObject toothObject = teethes[i];

                bool hasOutline = toothObject.TryGetComponent(out Outline outline);
                bool hasCollider = toothObject.TryGetComponent(out Collider collider);

                if (hasOutline == false)
                {
                    throw new NullReferenceException($"Add outline to {toothObject.name}");
                }

                if (hasCollider == false)
                {
                    throw new NullReferenceException($"Add collider to {toothObject.name}");
                }

                data.Add(toothObject.GetHashCode(), 
                    new ToothView(outline, collider, toothObject.transform));
            }

            return new MouthView(inputService, data, mouthTransform, dragParentTransform);
        }
    }

    public class MouthView : ActivateObject
    {
        private readonly IReadOnlyDictionary<int, ToothView> _teethes;

        private readonly SM _sm;

        private readonly HashSet<ToothView> _choosedTeeth;
        private ToothView _lastSelectedTooth;


        public MouthView(IInputService inputService, IReadOnlyDictionary<int, ToothView> teethes,
            UnityTransform mouthTransform, UnityTransform dragParentTransform)
        {
            _teethes = teethes;
            _choosedTeeth = new HashSet<ToothView>();

            _sm = new SM();
            _sm.AddStatementsRange
            (
                new MouthViewChooseToothState(_choosedTeeth, inputService, () => _lastSelectedTooth, _sm),
                new MouthViewDragTeethState(inputService, _choosedTeeth,mouthTransform, dragParentTransform, 
                    foo => _lastSelectedTooth = foo, _sm)
            );

            _sm.SwitchState<MouthViewChooseToothState>();

            SetActive(false);
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            
            foreach (var teeth in _teethes.Values)
            {
                teeth.SetActive(isActive);
            }

            if (isActive)
            {
                _sm.StartOrContinueMachine();
            }
            else
            {
                _sm.PauseMachine();
            }
        }
        public void SelectTooth(GameObject hittedObject)
        {
            if(hittedObject == null || _teethes.ContainsKey(hittedObject.GetHashCode()) == false)
            {
                _lastSelectedTooth?.Unselect();
                _lastSelectedTooth = null;

                return;
            }

            var newSelected = _teethes[hittedObject.GetHashCode()];

            if(ReferenceEquals(newSelected, _lastSelectedTooth)) return;

            _lastSelectedTooth?.Unselect();
            _lastSelectedTooth = newSelected;
            
            _lastSelectedTooth.Select();
        }
    }
}