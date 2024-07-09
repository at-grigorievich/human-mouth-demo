using ATG.Extensions;
using UnityEngine;

using UnityTransform = UnityEngine.Transform;

namespace ATG.Transform
{
    public sealed class OnlyRotateTransformBehaviour : ITransformBehaviour
    {
        private static float YRotateAxisRange = 80f;

        private readonly UnityTransform _transform;
        private readonly TransformData _config;

        public UnityTransform Transform => _transform;

        public OnlyRotateTransformBehaviour(UnityTransform transform, TransformData config)
        {
            _transform = transform;
            _config = config;
        }

        public void Move(Vector2 direction) { }

        public void Rotate(Vector2 mousePos)
        {
            Vector3 rotateEuler = _transform.localEulerAngles + new Vector3(-mousePos.y, mousePos.x, 0f);
            rotateEuler.x = rotateEuler.x.ClampAngle(-YRotateAxisRange, YRotateAxisRange);

            _transform.localEulerAngles = rotateEuler;
        }
    }
}