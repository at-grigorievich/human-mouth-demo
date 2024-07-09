using UnityEngine;

namespace ATG.Transform
{
    [CreateAssetMenu(menuName ="Config/New Transform Config", fileName = "transform_config")]
    public class TransformData: ScriptableObject
    {
        [field: SerializeField] public float SetPositionSpeed {get; private set;}
        [field: SerializeField] public float SetRotationSpeed {get; private set;}
    }
}