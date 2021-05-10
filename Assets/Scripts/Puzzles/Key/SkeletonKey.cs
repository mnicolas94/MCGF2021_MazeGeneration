using System;
using UI;
using UnityEngine;

namespace Puzzles.Key
{
    public class SkeletonKey : MonoBehaviour
    {
        public Action eventInteracted;
        
        [SerializeField] private Interactable interactable;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite skeletonWithoutKey;

        private void Start()
        {
            interactable.eventInteracted.AddListener(OnInteraction);
        }

        private void OnInteraction()
        {
            eventInteracted?.Invoke();
            spriteRenderer.sprite = skeletonWithoutKey;
            interactable.gameObject.SetActive(false);
        }
    }
}