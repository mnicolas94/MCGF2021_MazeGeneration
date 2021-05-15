using System;
using Character;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/HealthPotion", order = 0)]
    public class HealthPotion : Item
    {
        [SerializeField] private int health;
        
        public override void ApplyEffectOnPickUp()
        {
            var playerHealth = GameManager.Instance.PlayerController.GetComponent<Health>();
            playerHealth.Heal(health);
        }
    }
}