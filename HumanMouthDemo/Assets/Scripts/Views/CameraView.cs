using System;
using ATG.Activation;
using ATG.Input;
using ATG.Raycasting;
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
        [SerializeField] private RaycastData raycastData;

        public CameraView Create(IInputService inputService, MouthView mouthView)
        {
            IRaycastService raycastService = new RaycastService(camera.transform, raycastData);
            
            return new CameraView(camera, mouthView, cameraTransformConfig, inputService, raycastService);
        }
    }

    public class CameraView : ActivateObject, IUpdateable
    {
        private readonly Camera _camera;
        private readonly IRaycastHandler _toothRaycastHandler;
        private readonly SM _sm;

        public CameraView(Camera camera, MouthView mouthView, TransformData transformConfig, 
                IInputService inputService, IRaycastService raycastService)
        {
            _camera = camera;

            ITransformBehaviour transformBehaviour = 
                new DefaultTransformBehaviour(_camera.transform, transformConfig);
                
            _toothRaycastHandler = new ToothRaycastHandler(raycastService, mouthView);

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

            if(isActive == true)
            {
                _sm.StartOrContinueMachine();
            }
            else
            {
                _sm.PauseMachine();
            }

            _camera.enabled = isActive;
        }

        public void Update()
        {
            if(IsActive == false) return;

            _sm.ExecuteMachine();

            _toothRaycastHandler.Update();
        }
    }
}