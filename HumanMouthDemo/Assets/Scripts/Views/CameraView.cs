using System;
using ATG.Activation;
using ATG.Input;
using ATG.StateMachine.Views;
using ATG.Transform;
using ATG.Update;
using UnityEngine;

using SM = ATG.StateMachine.StateMachine;

namespace ATG.Views
{
    [Serializable]
    public sealed class CameraViewFactory
    {
        [SerializeField] private Camera camera;
        [SerializeField] private TransformData cameraTransformConfig;

        public CameraView Create(IInputService inputService) =>
            new CameraView(camera, cameraTransformConfig, inputService);
    }

    public class CameraView : ActivateObject, IUpdateable
    {
        private readonly Camera _camera;

        private readonly SM _sm;

        public CameraView(Camera camera, TransformData transformConfig, 
                                                IInputService inputService)
        {
            _camera = camera;

            ITransformBehaviour transformBehaviour = 
                new DefaultTransformBehaviour(_camera.transform, transformConfig);
            
            _sm = new SM();
            _sm.AddStatementsRange
            (
                new CameraViewIdleState(inputService, _sm),
                new CameraViewMoveState(transformBehaviour, inputService, _sm)
            );

            _sm.SwitchState<CameraViewIdleState>();

            SetActive(false);
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _camera.enabled = isActive;

            if(isActive == true)
            {
                _sm.StartOrContinueMachine();
            }
            else
            {
                _sm.PauseMachine();
            }
        }

        public void Update()
        {
            if(IsActive == false) return;

            _sm.ExecuteMachine();
        }
    }
}