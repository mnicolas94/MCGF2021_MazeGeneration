using System;
using System.Collections;
using Character;
using Character.Controllers;
using Dialogues;
using UI;
using UnityEngine;

namespace Puzzles.King
{
    public class King : MonoBehaviour
    {
        [SerializeField] private DialogueSequence kingDialogue;
        [SerializeField] private Interactable interactable;
        
        [SerializeField] private float timeAfterBattleToNotifyPuzzleSolved;

        [SerializeField] private string minotaurTag;
        private GameObject _minotaur;
        
        private IEnumerator Start()
        {
            interactable.eventInteracted.AddListener(OnInteracted);

            yield return null;

            _minotaur = GameObject.FindGameObjectWithTag(minotaurTag);
            _minotaur.SetActive(false);
            var minotaurHealth = _minotaur.GetComponent<Health>();
            minotaurHealth.eventDied += OnMinotaurDead;
        }

        private void OnInteracted()
        {
            interactable.gameObject.SetActive(false);
            // push dialogue
            
            // wait for dialogue and then open door
            var door = FindObjectOfType<KingWallDoor>();
            door.Open(() =>
            {
                _minotaur.SetActive(true);
            });
        }
        
        private void OnMinotaurDead()
        {
            var coroutine = WaitForBattleToEnd(GameManager.Instance.NotifyPuzzleSolved);
            StartCoroutine(coroutine);
        }

        private IEnumerator WaitForBattleToEnd(Action onBattleEnd)
        {
            while (GameManager.Instance.BattleController.IsRunningBattle)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(timeAfterBattleToNotifyPuzzleSolved);
            
            onBattleEnd?.Invoke();
        }
    }
}