using System;
using System.Collections;
using Puzzles.LeversPuzzle;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Puzzles.Combinations
{
    public class CombinationLever : MonoBehaviour
    {
        private static readonly int OpenedParameterHash = Animator.StringToHash("opened");
        public static int ActivateStateHashId = Animator.StringToHash("pulled");

        public Action eventLeverStateChanged;
        public UnityEvent eventOpenedOrClosed;
        
        [SerializeField] private Interactable interactable;
        [SerializeField] private ShowHidePanel panel;
        [SerializeField] private TextMeshProUGUI numberText;
        
        [SerializeField] private Animator wallAnimator;
        [SerializeField] private Animator leverAnimator;
        
        [SerializeField] private Button leverButton;

        private bool _activated;

        public bool Activated => _activated;
        
        public int Number => int.Parse(numberText.text);

        private void Start()
        {
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
            bool closed = !wallAnimator.GetBool(OpenedParameterHash);
            if (closed)
            {
                wallAnimator.SetBool(OpenedParameterHash, true);
                interactable.gameObject.SetActive(true);
                eventOpenedOrClosed?.Invoke();
            }
        }
        
        public void Close()
        {
            bool opened = wallAnimator.GetBool(OpenedParameterHash);
            if (opened)
            {
                wallAnimator.SetBool(OpenedParameterHash, false);
                interactable.gameObject.SetActive(false);
                eventOpenedOrClosed?.Invoke();
            }
        }

        public void ClosePanel()
        {
            panel.HidePanel();
        }
    }
}