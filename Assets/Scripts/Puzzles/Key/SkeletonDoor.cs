using System;
using System.Collections;
using Items;
using Items.Implementations;
using UI;
using UnityEngine;

namespace Puzzles.Key
{
    public class SkeletonDoor : MonoBehaviour
    {
        [SerializeField] private DoNothingItem keyItem;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Interactable doorInteractable;
        [SerializeField] private ShowHidePanel messagePanel;
        
        private SkeletonKey _skeletonKey;

        private IEnumerator Start()
        {
            doorInteractable.eventInteracted.AddListener(OnInteractedWithDoor);
            doorInteractable.eventLeavedInteractionArea.AddListener(messagePanel.HidePanel);
            
            yield return null;

            _skeletonKey = FindObjectOfType<SkeletonKey>();
            _skeletonKey.eventInteracted += OnInteractedWithSkeleton;
        }

        private void OnInteractedWithDoor()
        {
            if (inventory.HasItem(keyItem))
            {
                GameManager.Instance.NotifyPuzzleSolved();
            }
            else
            {
                messagePanel.ShowPanel();
            }
        }

        private void OnInteractedWithSkeleton()
        {
            inventory.AddItem(keyItem);
        }
    }
}