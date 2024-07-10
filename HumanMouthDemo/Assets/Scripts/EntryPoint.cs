#nullable enable

using System;
using ATG.DTO;
using ATG.Extensions;
using ATG.Input;
using ATG.Serialization;
using ATG.UI;
using ATG.Update;
using ATG.Views;
using DG.Tweening;
using UnityEngine;

namespace ATG.MouthTrainer
{
    public sealed class EntryPoint : MonoBehaviour, IDisposable
    {
        [SerializeField] private UpdateService updService;
        [SerializeField] private UIManagerFactory uiManagerFactory;
        [SerializeField] private CameraViewFactory cameraViewFactory;
        [SerializeField] private MouthViewFactory mouthViewFactory;
        
        private UIManager _uiManager = null!;

        private CameraView _cameraViewInstance = null!;
        private MouthView _mouthViewInstance = null!;

        private InputService _inputService = null!;

        private void Awake()
        {
            InputControl inputControl = new InputControl();
            _inputService = new InputService(inputControl);

            _uiManager = uiManagerFactory.Create();

            _mouthViewInstance = mouthViewFactory.Create(_inputService);
            _cameraViewInstance = cameraViewFactory.Create(_inputService, _mouthViewInstance);
            
            DOTween.Init();
        }

        private void Start()
        {
            updService.Add(_cameraViewInstance);
            updService.Add(_mouthViewInstance);

            _inputService.SetActive(true);
            _cameraViewInstance.SetActive(true);
            _mouthViewInstance.SetActive(true);

            updService.SetActive(true);

            _uiManager.AddQuitButtonCallback(Quit);
            _uiManager.AddResetButtonCallback(_mouthViewInstance.Reset);
            _uiManager.AddSaveButtonCallback(
                () => BinnarySerializationService
                .Write(MouthTransformDTO.FilePath, _mouthViewInstance.GetDataTrasfer()));

            _uiManager.SetActive(true);
        }

        private void Quit()
        {
            Dispose();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Dispose()
        {
            _inputService.SetActive(false);
            _cameraViewInstance.SetActive(false);
            _mouthViewInstance.SetActive(false);

            _uiManager.SetActive(false);

            updService.SetActive(false);
        }

        [ContextMenu("Remove saves")]
        public void RemoveSaves()
        {
            BinnarySerializationService.Delete(MouthTransformDTO.FilePath);
        }
    }
}