using System;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public Action<int, int> eventMaxHealthChanged;
        
        [SerializeField] private int maxHealth;
        
        [Range(0.0f, 1.0f)]
        [SerializeField] private float attackChance;

        public int MaxHealth => maxHealth;

        public void SetMaxHealth(int newMax)
        {
            int oldMax = maxHealth;
            maxHealth = newMax;

            if (oldMax != newMax)
            {
                eventMaxHealthChanged?.Invoke(oldMax, newMax);
            }
        }
        
    }
}