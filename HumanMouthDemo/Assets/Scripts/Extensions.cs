using UnityEngine;

namespace ATG.Extensions
{
    public static class Extensions
    {
        public static float ClampAngle(this float angle, float from, float to)
            {
                if (angle < 0f) angle = 360 + angle;
                if (angle > 180f) return Mathf.Max(angle, 360 + from);
                return Mathf.Min(angle, to);
            }
    }
}