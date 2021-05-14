using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogues.UI
{
    public class DialoguePanel : MonoBehaviour
    {
        public Action eventAllDialoguesProcessed;
        public Action eventDialogueProcessed;
        
        [SerializeField] private Sprite emptySprite;

        [SerializeField] private GameObject panelContainer;
        [SerializeField] private Image speakerSprite;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private DialogueText dialogueText;
        [SerializeField] private float processTimeOut;
        
        [SerializeField] private KeyCode nextKey;
        
        private List<Dialogue> _dialogueQueue;
        private float _nextTimeToProcess;
        private bool _showingDialogue;

        public bool ShowingDialogue => _showingDialogue;

        private void Awake()
        {
            _dialogueQueue = new List<Dialogue>();
            ProcessQueue(false);
            _nextTimeToProcess = Time.time + processTimeOut;
        }

        private bool ShouldProcessOnPush()
        {
            bool shouldProcess = _dialogueQueue.Count == 0 && !dialogueText.IsRunning;
            return shouldProcess;
        }
        
        public void PushDialogue(Dialogue dialogue)
        {
            bool shouldProcess = ShouldProcessOnPush();
            _dialogueQueue.Add(dialogue);

            if (shouldProcess)
            {
                ProcessQueue();
            }
        }

        public void PushDialogueSequence(DialogueSequenceBase sequence, bool clearOthers = true)
        {
            if (clearOthers)
            {
                ClearQueue();
            }

            bool shouldProcess = ShouldProcessOnPush();

            foreach (var dialogue in sequence.Dialogues())
            {
                _dialogueQueue.Add(dialogue);
            }
            
            if (shouldProcess)
            {
                ProcessQueue();
            }
        }

        public void ClearQueue()
        {
            _dialogueQueue.Clear();
            ProcessQueue(false);
            dialogueText.ShowAll();
        }

        private void ProcessQueue(bool emitEvents = true)
        {
            if (_dialogueQueue.Count > 0)
            {
                panelContainer.SetActive(true);
                var dialogue = _dialogueQueue[0];
                _dialogueQueue.RemoveAt(0);

                speakerNameText.text = dialogue.Speaker.CharacterName;
                speakerNameText.color = dialogue.Speaker.NameColor;
                dialogueText.PutText(dialogue.Text);

                _nextTimeToProcess = Time.time + processTimeOut;
                _showingDialogue = true;
                
                if (emitEvents)
                {
                    eventDialogueProcessed?.Invoke();
                }
            }
            else
            {
//                speakerSprite.sprite = emptySprite;
                speakerNameText.text = "";
                dialogueText.PutText("");
                panelContainer.SetActive(false);

                _showingDialogue = false;
                
                if (emitEvents)
                {
                    eventAllDialoguesProcessed?.Invoke();
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(nextKey))
            {
                if (dialogueText.IsRunning)
                {
                    dialogueText.ShowAll();
                }
                else
                {
                    ProcessQueue();
                }
            }

            if (Time.time >= _nextTimeToProcess && ShowingDialogue)
            {
                ProcessQueue();
            }
        }
    }
}