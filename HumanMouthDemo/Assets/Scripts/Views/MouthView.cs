#nullable enable

using System;
using System.Collections.Generic;
using ATG.Activation;
using ATG.Input;
using ATG.StateMachine.Views;
using ATG.Transform;
using ATG.Update;
using ATG.DTO;
using ATG.Serialization;
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
            
            MouthTransformDTO? dto = BinnarySerializationService.Read<MouthTransformDTO>(MouthTransformDTO.FilePath);
            dto?.SetupTransform(mouthTransform);

            ITransformBehaviour transformBehaviour = new OnlyRotateTransformBehaviour(mouthTransform, config);

            return new MouthView(inputService, transformBehaviour, data, mouthTransform, dragParentTransform);
        }
    }

    public class MouthView : ActivateObject, IUpdateable
    {
        private readonly TeethSet _set;

        private readonly SM _sm;
        private readonly SM _transformSM;

        public readonly UnityTransform Transform;
        
        public MouthView(IInputService inputService, ITransformBehaviour transformBehaviour,
            IReadOnlyDictionary<int, ToothView> teeth,
            UnityTransform mouthTransform, UnityTransform dragParentTransform)
        {
            Transform = mouthTransform;

            _set = new TeethSet(teeth);
            _sm = new SM();
            _transformSM = new SM();

            _sm.AddStatementsRange
            (
                new MouthViewChooseToothState(_set, inputService, _sm),
                new MouthViewDragTeethState(inputService, _set, dragParentTransform, _sm),
                new MouthViewResetToothState(_set, mouthTransform, _sm)
            );

            _transformSM.AddStatementsRange(new MouthViewRotateState(transformBehaviour, inputService, _sm));

            _sm.SwitchState<MouthViewChooseToothState>();
            _transformSM.SwitchState<MouthViewRotateState>();

            SetActive(false);
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _set.SetActive(isActive);

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
            _set.Reset();
            _sm.SwitchState<MouthViewResetToothState>();
        }
        public void SelectTooth(GameObject hittedObject) => _set.SelectTooth(hittedObject);

    }
}