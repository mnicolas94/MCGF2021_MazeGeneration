using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PuzzlesUI.NumbersPuzzle
{
    public class NumbersPuzzleInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ShowPanelOnInteraction panel;

        private NumbersPuzzleDiagram _diagram;

        private IEnumerator Start()
        {
            inputField.onValueChanged.AddListener(OnTextChanged);
            panel.eventShowed += SetInputFocus;
            yield return null;  // wait one frame

            _diagram = FindObjectOfType<NumbersPuzzleDiagram>();
        }

        private void OnTextChanged(string text)
        {
            if (text == _diagram.GeneratedSequence)
            {
                panel.HidePanel();
                GameManager.Instance.NotifyPuzzleSolved();
            }
        }

        private void SetInputFocus()
        {
            inputField.Select();
        }
    }
}