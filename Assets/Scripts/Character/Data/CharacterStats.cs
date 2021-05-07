using System;
using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public Action<int, int> eventMaxHealthChanged;
        
        [SerializeField] private int maxHealth;
        
        [SerializeField] [Range(0.0f, 1.0f)] private float firstHitChance;
        [SerializeField] [Range(0.0f, 1.0f)] private float hitChance;

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

        public float FirstHitChance => firstHitChance;

        public float HitChance => hitChance;
    }
}