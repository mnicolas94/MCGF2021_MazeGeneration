using System;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzles.LeversPuzzle
{
    public class Lever : MonoBehaviour
    {
        public static int ActivateStateHashId = Animator.StringToHash("pulled");

        public Action eventLeverStateChanged;

        [SerializeField] private Animator animator;

        [SerializeField] private Button leverButton;

        private bool _activated;

        public bool Activated => _activated;

        private void Start()
        {
            leverButton.onClick.AddListener(OnLeverClicked);
        }

        private void OnLeverClicked()
        {
            _activated = !_activated;
            animator.SetBool(ActivateStateHashId, _activated);
            
            eventLeverStateChanged?.Invoke();
        }

        public void Disable()
        {
            leverButton.interactable = false;
        }
    }
}