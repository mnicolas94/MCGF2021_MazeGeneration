using Character.Data;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "Sword", menuName = "Items/Sword", order = 0)]
    public class Sword : Item
    {
        [SerializeField] private CharacterStats mainCharacterStats;
        [SerializeField] [Range(0.0f, 1.0f)] private float chance;
        
        public override void ApplyEffectOnPickUp()
        {
            mainCharacterStats.SetHitChance(chance);
        }
    }
}