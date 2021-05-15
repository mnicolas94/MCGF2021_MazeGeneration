using Character;
using UnityEngine;
using Utils;

namespace Traps
{
    public class HealOnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private int heal;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (playerMask.IsLayerInMask(otherLayer))
            {
                var health = other.GetComponent<Health>();
                health.Heal(heal);
                Destroy(gameObject);
            }
        }
    }
}