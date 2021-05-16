using System;
using System.Collections;
using Dialogues;
using Dialogues.UI;
using Items;
using Items.Implementations;
using UI;
using UnityEngine;

namespace Puzzles.Key
{
    public class SkeletonDoor : MonoBehaviour
    {
        [SerializeField] private PuzzleData puzzle;

        [Space] [SerializeField] private DialogueSequenceBase interactionDialogue;
        [SerializeField] private Item keyItem;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Interactable doorInteractable;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        
        private SkeletonKey _skeletonKey;

        private IEnumerator Start()
        {
            GameManager.Instance.eventFinishedLevel += OnLevelFinished;
            doorInteractable.eventInteracted.AddListener(OnInteractedWithDoor);
            
            yield return null;

            _skeletonKey = FindObjectOfType<SkeletonKey>();
            _skeletonKey.eventInteracted += OnInteractedWithSkeleton;
        }

        private void OnInteractedWithDoor()
        {
            if (inventory.HasItem(keyItem))
            {
                inventory.RemoveItem(keyItem);
                audioSource.PlayOneShot(clip);
                GameManager.Instance.NotifyPuzzleSolved(puzzle);
            }
            else
            {
                GameManager.Instance.DialoguePanel.PushDialogueSequence(interactionDialogue);
            }
        }

        private void OnInteractedWithSkeleton()
        {
            inventory.AddItem(keyItem);
        }

        private void OnLevelFinished()
        {
            inventory.RemoveItem(keyItem);
        }
    }
}