using System;
using UI;
using UnityEngine;

namespace Puzzles.Key
{
    public class SkeletonKey : MonoBehaviour
    {
        public Action eventInteracted;
        
        [SerializeField] private Interactable interactable;

        private void Start()
        {
            interactable.eventInteracted.AddListener(OnInteraction);
        }

        private void OnInteraction()
        {
            eventInteracted?.Invoke();
        }
    }
}