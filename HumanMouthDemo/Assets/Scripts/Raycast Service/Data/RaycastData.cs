using UnityEngine;

namespace ATG.Raycasting
{
    [CreateAssetMenu(menuName = "Config/New Raycast Config", fileName = "new_raycast_config")]
    public class RaycastData: ScriptableObject
    {
        [field: SerializeField] public float RaycastDistance {get; private set;}
        [SerializeField] private string LayerName;

        public int Layer => LayerMask.GetMask(LayerName);
    }
}