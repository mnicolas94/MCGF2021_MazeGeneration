using Character;
using Character.Data;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "MaxHealthRaiser", menuName = "Items/MaxHealthRaiser", order = 0)]
    public class MaxHealthRaiser : Item
    {
        [SerializeField] private int healthAmount;
        
        public override void ApplyEffectOnPickUp()
        {
            var playerHealth = GameManager.Instance.PlayerController.GetComponent<Health>();
            playerHealth.SetMaxHealth(playerHealth.MaxHealth + healthAmount);
        }
    }
}