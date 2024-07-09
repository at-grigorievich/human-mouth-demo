using System;
using System.Collections.Generic;
using ATG.Activation;
using ATG.Input;
using ATG.StateMachine.Views;
using UnityEngine;

using UnityTransform = UnityEngine.Transform;
using SM = ATG.StateMachine.StateMachine;
using ATG.Transform;
using ATG.Update;

namespace ATG.Views
{
    [Serializable]
    public sealed class MouthViewFactory
    {
        [SerializeField] private UnityTransform mouthTransform;
        [SerializeField] private UnityTransform dragParentTransform;
        [Space(15)]
        [SerializeField] private TransformData config;
        [Space(15)]
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

            ITransformBehaviour transformBehaviour = new OnlyRotateTransformBehaviour(mouthTransform, config);

            return new MouthView(inputService, transformBehaviour, data, mouthTransform, dragParentTransform);
        }
    }

    public class MouthView : ActivateObject, IUpdateable
    {
        private readonly IReadOnlyDictionary<int, ToothView> _teethes;

        private readonly SM _sm;
        private readonly SM _transformSM;

        private readonly HashSet<ToothView> _choosedTeeth;
        private ToothView _lastSelectedTooth;

        public MouthView(IInputService inputService, ITransformBehaviour transformBehaviour,
            IReadOnlyDictionary<int, ToothView> teethes,
            UnityTransform mouthTransform, UnityTransform dragParentTransform)
        {
            _teethes = teethes;
            _choosedTeeth = new HashSet<ToothView>();

            _sm = new SM();
            _transformSM = new SM();

            _sm.AddStatementsRange
            (
                new MouthViewChooseToothState(_choosedTeeth, inputService, () => _lastSelectedTooth, _sm),
                new MouthViewDragTeethState(inputService, _choosedTeeth, dragParentTransform,
                    foo => _lastSelectedTooth = foo, _sm),
                new MouthViewResetToothState(_teethes.Values, mouthTransform, _sm)
            );

            _transformSM.AddStatementsRange(new MouthViewRotateState(transformBehaviour, inputService, _sm));

            _sm.SwitchState<MouthViewChooseToothState>();
            _transformSM.SwitchState<MouthViewRotateState>();

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
                _transformSM.StartOrContinueMachine();
            }
            else
            {
                _sm.PauseMachine();
                _transformSM.PauseMachine();
            }
        }

        public void Update()
        {
            if (IsActive == false) return;

            _sm.ExecuteMachine();
            _transformSM.ExecuteMachine();
        }

        public void Reset()
        {
            _choosedTeeth.Clear();
            _lastSelectedTooth = null;
            _sm.SwitchState<MouthViewResetToothState>();
        }
        public void SelectTooth(GameObject hittedObject)
        {
            if (hittedObject == null || _teethes.ContainsKey(hittedObject.GetHashCode()) == false)
            {
                _lastSelectedTooth?.Unselect();
                _lastSelectedTooth = null;

                return;
            }

            var newSelected = _teethes[hittedObject.GetHashCode()];

            if (ReferenceEquals(newSelected, _lastSelectedTooth)) return;

            _lastSelectedTooth?.Unselect();
            _lastSelectedTooth = newSelected;

            _lastSelectedTooth.Select();
        }

    }
}