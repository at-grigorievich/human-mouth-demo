using UnityEngine;

namespace ATG.Transform
{
    public interface ITransformBehaviour
    {
        UnityEngine.Transform Transform {get;}

        void Move(Vector2 direction);
        void Rotate(Vector2 direction);
    }
}
