using System;
using Character;
using UI;
using UnityEngine;

namespace Puzzles.BloodSacrifice
{
    public class BloodSacrifice : MonoBehaviour
    {
        [SerializeField] private Interactable interactable;
        [SerializeField] private int damage;
        
        private void Start()
        {
            interactable.eventInteracted.AddListener(OnInteraction);
        }

        private void OnInteraction()
        {
            interactable.gameObject.SetActive(false);
            var health = GameManager.Instance.PlayerController.GetComponent<Health>();
            health.DoDamage(damage);
            GameManager.Instance.NotifyPuzzleSolved();
        }
    }
}