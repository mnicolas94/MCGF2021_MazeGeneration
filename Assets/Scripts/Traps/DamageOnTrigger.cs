using System;
using Character;
using UnityEngine;
using Utils;

namespace Traps
{
    public class DamageOnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private int damage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (playerMask.IsLayerInMask(otherLayer))
            {
                var health = other.GetComponent<Health>();
                health.DoDamage(damage);
            }
        }
    }
}