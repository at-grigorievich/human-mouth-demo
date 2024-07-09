#nullable enable

using ATG.Input;
using ATG.Update;
using ATG.Views;
using UnityEngine;

namespace ATG.MouthTrainer
{
    public sealed class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UpdateService updService;
        [SerializeField] private CameraViewFactory cameraViewFactory;
        [SerializeField] private MouthViewFactory mouthViewFactory;

        private CameraView _cameraViewInstance = null!;
        private MouthView _mouthViewInstance = null!;

        private InputService _inputService = null!;
    
        private void Awake()
        {
            InputControl inputControl = new InputControl();
            _inputService = new InputService(inputControl);

            _mouthViewInstance = mouthViewFactory.Create(_inputService);
            _cameraViewInstance = cameraViewFactory.Create(_inputService, _mouthViewInstance);
        }

        private void Start()
        {
            updService.Add(_cameraViewInstance);

            _inputService.SetActive(true);
            _cameraViewInstance.SetActive(true);
            _mouthViewInstance.SetActive(true);

            updService.SetActive(true);
        }
    }
}