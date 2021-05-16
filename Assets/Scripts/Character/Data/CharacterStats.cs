using System;
using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ResetableScriptableObject
    {
        public Action<int, int> eventMaxHealthChanged;
        
        [SerializeField] private int maxHealth;
        private int _maxHealthRuntime;
        [SerializeField] [Range(0.0f, 1.0f)] private float firstHitChance;
        [SerializeField] [Range(0.0f, 1.0f)] private float hitChance;
        private float _hitChanceRuntime;

        public int MaxHealth => _maxHealthRuntime;

        public float FirstHitChance => firstHitChance;

        public float HitChance => _hitChanceRuntime;

        public void SetMaxHealth(int newMax)
        {
            _maxHealthRuntime = newMax;
        }
        
        public void SetHitChance(float chance)
        {
            _hitChanceRuntime = chance;
        }

        public override void ResetData()
        {
            _maxHealthRuntime = maxHealth;
            _hitChanceRuntime = hitChance;
        }

        private void OnEnable()
        {
            ResetData();
        }
    }
}