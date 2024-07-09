using System;
using ATG.Activation;
using DG.Tweening;
using UnityEngine;

using UnityTransform = UnityEngine.Transform;

namespace ATG.Views
{
    public class ToothView : ActivateObject
    {
        private readonly Outline _outline;
        private readonly Collider _collider;
        private readonly ToothViewTweener _tweener;

        private bool _isChoosed;
        private bool _isSelected;

        public UnityTransform ToothTransfrom { get; private set; }

        public ToothView(Outline outline, Collider collider, UnityTransform toothTransfrom)
        {
            _outline = outline;
            _collider = collider;

            _outline.OutlineColor = Color.green;

            ToothTransfrom = toothTransfrom;

            _tweener = new ToothViewTweener(ToothTransfrom);
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

            _tweener.Dispose();
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

                StopAnimate();
            }
        }

        public void StartAnimate() => _tweener.Shake();
        public void StopAnimate() => _tweener.Dispose();

        private sealed class ToothViewTweener : IDisposable
        {
            private const float _duration = 1f;
            private const Ease _ease = Ease.Linear;

            private readonly UnityTransform _ownerTransform;

            private readonly Vector3 _originalLocalPosition;
            private readonly Vector3 _originalLocalEulerAngles;

            private Tween _tween;

            public ToothViewTweener(UnityTransform owner)
            {
                _ownerTransform = owner;

                _originalLocalPosition = _ownerTransform.localPosition;
                _originalLocalEulerAngles = _ownerTransform.localEulerAngles;
            }

            public void Dispose()
            {
                _tween?.Kill();
                _tween = null;
            }

            public void Shake()
            {
                Dispose();
                _tween = _ownerTransform.DOShakeRotation(_duration, 1f, 10)
                                .SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}