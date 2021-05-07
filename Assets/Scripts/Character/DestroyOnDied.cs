using System;
using UnityEngine;

namespace Character
{
    public class DestroyOnDied : MonoBehaviour
    {
        [SerializeField] private Health health;

        private void Start()
        {
            health.eventDied += OnDead;
        }

        private void OnDead()
        {
            Destroy(gameObject);
        }
    }
}