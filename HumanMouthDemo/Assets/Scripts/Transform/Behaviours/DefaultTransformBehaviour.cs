using UnityEngine;

using UnityTransform = UnityEngine.Transform;

namespace ATG.Transform
{
    public sealed class DefaultTransformBehaviour : ITransformBehaviour
    {
        public static float YRotateAxisRange = 80f;

        private readonly UnityTransform _transform;
        private readonly TransformData _config;

        public UnityTransform Transform => _transform;

        public DefaultTransformBehaviour(UnityTransform transform, TransformData config)
        {
            _transform = transform;
            _config = config;
        }

        public void Move(Vector2 direction)
        {
            Vector3 forward = _transform.forward * direction.y;
            Vector3 right = _transform.right * direction.x;

            Vector3 nextTarget = _transform.position + forward + right;

            _transform.position = Vector3.MoveTowards(_transform.position, nextTarget,
                                                _config.SetPositionSpeed * Time.deltaTime);
        }

        public void Rotate(Vector2 mousePos)
        {
            Vector3 rotateVector = _transform.localEulerAngles + new Vector3(-mousePos.y, mousePos.x, 0f);

            Quaternion res = Quaternion.RotateTowards(_transform.localRotation, Quaternion.Euler(rotateVector),
                                                                            _config.SetRotationSpeed * Time.deltaTime);

            Vector3 clampedEulerAngles = res.eulerAngles;

            clampedEulerAngles.x = ClampAngle(clampedEulerAngles.x, -YRotateAxisRange, YRotateAxisRange);

            _transform.localEulerAngles = clampedEulerAngles;

            float ClampAngle(float angle, float from, float to)
            {
                if (angle < 0f) angle = 360 + angle;
                if (angle > 180f) return Mathf.Max(angle, 360 + from);
                return Mathf.Min(angle, to);
            }
        }
    }
}