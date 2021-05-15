using System.Runtime.Serialization;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "Minimap", menuName = "Items/Minimap", order = 0)]
    public class Minimap : Item
    {
        public override void ApplyEffectOnPickUp()
        {
            var minimapBehaviour = FindObjectOfType<MinimapContainer>();
            minimapBehaviour.EnableMinimap();
        }
    }
}