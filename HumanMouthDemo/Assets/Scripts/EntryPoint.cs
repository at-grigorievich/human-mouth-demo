#nullable enable

using ATG.Input;
using ATG.Update;
using ATG.Views;
using UnityEngine;

namespace ATG.MonthTrainer
{
    public sealed class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UpdateService updService;
        [SerializeField] private CameraViewFactory cameraViewFactory;

        private CameraView _cameraViewInstance = null!;
        private InputService _movementInput = null!;

        private void Awake()
        {
            InputControl inputControl = new InputControl();
            _movementInput = new InputService(inputControl);

            _cameraViewInstance = cameraViewFactory.Create(_movementInput);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            updService.Add(_cameraViewInstance);

            _movementInput.SetActive(true);
            _cameraViewInstance.SetActive(true);
            
            updService.SetActive(true);
        }
    }
}