using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Items;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzles.ScatteredCode
{
    public class ScatteredCodePuzzle : MonoBehaviour
    {
        [SerializeField] private PuzzleData puzzle;
        
        [Space]
        
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ShowHidePanel panel;

        [Space] [SerializeField] private Inventory inventory;
        [SerializeField] private Item scatteredCodeItem;
        [SerializeField] private List<Sprite> fragmentsItemSprites;

        [SerializeField] private DialogueSequenceBase interactionDialogue;

        private string _generatedCode;
        private CodeFragment[] _codeFragments;
        private int _fragmentsPickedUp;
        
        private IEnumerator Start()
        {
            inputField.onValueChanged.AddListener(OnTextChanged);
            panel.eventShowed += SetInputFocus;
            panel.eventHiden += ShowInteractionDialogue;

            _generatedCode = GenerateCode();

            GameManager.Instance.eventFinishedLevel += OnFinishedLevel;

            yield return null;  // wait one frame for decorator to spawn fragments

            _codeFragments = FindObjectsOfType<CodeFragment>();
            _fragmentsPickedUp = 0;
            foreach (var fragment in _codeFragments)
            {
                fragment.eventPickedUp += OnCodeFragmentPickedUp;
            }
        }

        private void OnFinishedLevel()
        {
            inventory.RemoveItem(scatteredCodeItem);
        }

        private void OnCodeFragmentPickedUp(CodeFragment fragment)
        {
            _fragmentsPickedUp++;
            if (_fragmentsPickedUp == 1)  // first fragment picked up
            {
                inventory.AddItem(scatteredCodeItem);
            }

            var sprite = fragmentsItemSprites[_fragmentsPickedUp - 1];
            scatteredCodeItem.SetSprite(sprite);
            string code = _generatedCode.Substring(0, _fragmentsPickedUp);
            scatteredCodeItem.SetUiText(code);
            
            inventory.UpdateItem(scatteredCodeItem);
        }

        private bool AllFragmentsPickedUp()
        {
            return _fragmentsPickedUp == _codeFragments.Length;
        }

        private void OnTextChanged(string text)
        {
            if (text == _generatedCode && AllFragmentsPickedUp())
            {
                float delayTime = 0.4f;
                panel.HidePanel(delayTime);
                GameManager.Instance.NotifyPuzzleSolved(puzzle, delayTime);
            }
        }
        
        private void SetInputFocus()
        {
            inputField.Select();
        }

        private void ShowInteractionDialogue()
        {
            if (!AllFragmentsPickedUp())
            {
                GameManager.Instance.DialoguePanel.PushDialogueSequence(interactionDialogue);
            }
        }

        private string GenerateCode()
        {
            int n1 = Random.Range(0, 10);
            int n2 = Random.Range(0, 10);
            int n3 = Random.Range(0, 10);
            int n4 = Random.Range(0, 10);

            return $"{n1}{n2}{n3}{n4}";
        }
    }
}