using System;
using ATG.Activation;
using UnityEngine;
using UnityEngine.UI;

namespace ATG.UI
{
    [Serializable]
    public sealed class UIManagerFactory
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button quitButton;

        public UIManager Create() =>
            new UIManager(saveButton, resetButton, quitButton);
    }

    [Serializable]
    public sealed class UIManager : ActivateObject
    {
        private readonly Button _saveButton;
        private readonly Button _resetButton;
        private readonly Button _quitButton;

        public UIManager(Button save, Button reset, Button quit)
        {
            _saveButton = save;
            _resetButton = reset;
            _quitButton = quit;

            SetActive(false);
        }

        public void AddSaveButtonCallback(Action callback) => _saveButton.onClick.AddListener(() => callback?.Invoke());
        public void AddResetButtonCallback(Action callback) => _resetButton.onClick.AddListener(() => callback?.Invoke());
        public void AddQuitButtonCallback(Action callback) => _quitButton.onClick.AddListener(() => callback?.Invoke());

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _saveButton.interactable = isActive;
            _resetButton.interactable = isActive;
            _quitButton.interactable = isActive;

            if (isActive == false)
            {
                _saveButton.onClick.RemoveAllListeners();
                _resetButton.onClick.RemoveAllListeners();
                _quitButton.onClick.RemoveAllListeners();
            }
        }
    }
}