using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "DoNothingItem", menuName = "Items/DoNothingItem", order = 0)]
    public class DoNothingItem : Item
    {
        public override void ApplyEffectOnPickUp()
        {
            // do nothing
        }
    }
}