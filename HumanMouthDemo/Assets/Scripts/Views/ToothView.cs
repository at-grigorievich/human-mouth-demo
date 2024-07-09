using ATG.Activation;
using UnityEngine;

using UnityTransform = UnityEngine.Transform;

namespace ATG.Views
{
    public class ToothView : ActivateObject
    {
        private readonly Vector3 _originalLocalPosition;
        private readonly Vector3 _originalLocalEulerAngles;

        private readonly Outline _outline;
        private readonly Collider _collider;

        private bool _isChoosed;
        private bool _isSelected;

        public UnityTransform ToothTransfrom {get; private set;}

        public ToothView(Outline outline, Collider collider, UnityTransform toothTransfrom)
        {
            _outline = outline;
            _collider = collider;

            _outline.OutlineColor = Color.green;

            ToothTransfrom = toothTransfrom;

            _originalLocalPosition = ToothTransfrom.localPosition; 
            _originalLocalEulerAngles = ToothTransfrom.localEulerAngles;
        }

        public void Select()
        {
            _outline.enabled = true;
            _isSelected = true;
        }

        public void Unselect()
        {
            if (_isChoosed == false)
            {
                _outline.enabled = false;
            }
            _isSelected = false;
        }

        public void Choose()
        {
            _outline.OutlineColor = Color.red;
            _isChoosed = true;
        }

        public void Unchoose()
        {
            _outline.OutlineColor = Color.green;
            _isChoosed = false;
        }

        public void SetParent(UnityTransform parent)
        {
            ToothTransfrom.SetParent(parent);
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            _collider.enabled = isActive;

            if (isActive == false)
            {
                Unselect();
                Unchoose();
            }
        }
    }
}