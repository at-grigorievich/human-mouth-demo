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
        }

        public void Unselect()
        {
            if (_isChoosed == false)
            {
                _outline.enabled = false;
            }
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

        #region work with tweener
        public void Reset(Action callback = null)
        {
            Unchoose();
            Unselect();
            _tweener.Reset(callback);
        }
        public void StartAnimate() => _tweener.Shake();
        public void StopAnimate() => _tweener.Dispose();

        private sealed class ToothViewTweener : IDisposable
        {
            private const float _shakeDuration = 1f;
            private const float _resetDuration = 0.4f;
            private const Ease _ease = Ease.Linear;

            private readonly UnityTransform _ownerTransform;

            private readonly Vector3 _originalLocalPosition;
            private readonly Vector3 _originalLocalEulerAngles;

            private Sequence _seq;

            public ToothViewTweener(UnityTransform owner)
            {
                _ownerTransform = owner;

                _originalLocalPosition = _ownerTransform.localPosition;
                _originalLocalEulerAngles = _ownerTransform.localEulerAngles;
            }

            public void Dispose()
            {
                _seq?.Kill();
                _seq = null;
            }

            public void Shake()
            {
                Dispose();
                _seq = DOTween.Sequence()
                    .Append(_ownerTransform.DOShakeRotation(_shakeDuration, 1f, 10))
                    .SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
            }

            public void Reset(Action callback)
            {
                Dispose();
                
                float distance = Vector3.Distance(_ownerTransform.localPosition, _originalLocalPosition);
                
                if(distance > Mathf.Epsilon)
                {
                    _seq = DOTween.Sequence()
                        .Append(_ownerTransform.DOLocalMove(_originalLocalPosition, _resetDuration))
                        .Join(_ownerTransform.DOLocalRotate(_originalLocalEulerAngles, _resetDuration))
                        .SetEase(_ease)
                        .OnComplete(() =>  callback?.Invoke());
                }
                else
                {
                    _ownerTransform.localPosition = _originalLocalPosition;
                    _ownerTransform.localRotation = Quaternion.Euler(_originalLocalEulerAngles);
                    callback?.Invoke();
                }
            }
        }
        #endregion
    }
}