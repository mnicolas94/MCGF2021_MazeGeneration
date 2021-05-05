using System;
using Puzzles.LeversPuzzle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzles.Combinations
{
    public class CombinationLever : MonoBehaviour
    {
        private static readonly int OpenedParameterHash = Animator.StringToHash("opened");
        public static int ActivateStateHashId = Animator.StringToHash("pulled");

        public Action eventLeverStateChanged;
        
        [SerializeField] private Interactable interactable;
        [SerializeField] private ShowPanelOnInteraction panel;
        [SerializeField] private TextMeshProUGUI numberText;
        
        [SerializeField] private Animator wallAnimator;
        [SerializeField] private Animator leverAnimator;
        
        [SerializeField] private Button leverButton;

        private bool _activated;

        public bool Activated => _activated;
        
        public int Number => int.Parse(numberText.text);

        private void Start()
        {
            Close();
            leverButton.onClick.AddListener(OnLeverClicked);
        }

        public void SetNumber(int number)
        {
            numberText.text = number.ToString();
        }
        
        private void OnLeverClicked()
        {
            _activated = !_activated;
            leverAnimator.SetBool(ActivateStateHashId, _activated);
            
            eventLeverStateChanged?.Invoke();
        }

        public void Open()
        {
            wallAnimator.SetBool(OpenedParameterHash, true);
            interactable.gameObject.SetActive(true);
        }
        
        public void Close()
        {
            wallAnimator.SetBool(OpenedParameterHash, false);
            interactable.gameObject.SetActive(false);
        }

        public void ClosePanel()
        {
            panel.HidePanel();
        }
    }
}