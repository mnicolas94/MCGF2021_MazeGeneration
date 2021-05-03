using System;
using UnityEngine;

namespace Character
{
    public class Health : MonoBehaviour
    {
        public Action<int> eventDamaged;
        public Action<int> eventHealed;
        public Action eventDied;
        
        [SerializeField] private CharacterStats stats;
        [SerializeField] private bool invulnerable;

        private int _currentHealth;

        public int CurrentHealth => _currentHealth;

        public bool Invulnerable => invulnerable;

        public void SetInvulnerable(bool inv)
        {
            invulnerable = inv;
        }

        private void Start()
        {
            _currentHealth = stats.MaxHealth;
            stats.eventMaxHealthChanged += OnMaxHealthChanged;
        }

        private void OnMaxHealthChanged(int oldMax, int newMax)
        {
            int difference = newMax - oldMax;
            
            if (difference > 0)
            {
                _currentHealth += difference;
            }
            else
            {
                _currentHealth = Math.Min(_currentHealth, newMax);
            }
        }
        
        public void DoDamage(int dmg)
        {
            if (dmg < 0 || invulnerable)
                return;
            
            int effectiveDamage = Math.Min(_currentHealth, dmg);
            _currentHealth -= effectiveDamage;

            if (effectiveDamage > 0)
            {
                eventDamaged?.Invoke(effectiveDamage);
            }

            if (_currentHealth <= 0)
            {
                eventDied?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            
            int effectiveHeal = Math.Min(stats.MaxHealth - _currentHealth, amount);
            _currentHealth += effectiveHeal;

            if (effectiveHeal > 0)
            {
                eventHealed?.Invoke(effectiveHeal);
            }
        }
    }
}