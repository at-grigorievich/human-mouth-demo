using System;
using UnityEngine;

namespace ATG.DTO
{
    [Serializable]
    public struct NumVector3
    {
        public float x;
        public float y;
        public float z;

        public NumVector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public UnityEngine.Vector3 ToUnityVector3() => new UnityEngine.Vector3(x, y, z);
    }

    [Serializable]
    public class MouthTransformDTO
    {
        public static string FilePath = Application.persistentDataPath + "/data.bf";

        public NumVector3 Position;
        public NumVector3 EulerAngles;

        public MouthTransformDTO(Vector3 pos, Vector3 eulerAngles)
        {
            Position = new NumVector3(pos.x, pos.y, pos.z);
            EulerAngles = new NumVector3(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public MouthTransformDTO(NumVector3 position, NumVector3 eulerAngles)
        {
            Position = position;
            EulerAngles = eulerAngles;
        }

        public void SetupTransform(UnityEngine.Transform transform)
        {
            transform.localPosition = Position.ToUnityVector3();
            transform.localEulerAngles = EulerAngles.ToUnityVector3();
        }
    }
}