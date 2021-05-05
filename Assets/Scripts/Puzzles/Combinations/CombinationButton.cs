using System;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzles.Combinations
{
    public class CombinationButton : MonoBehaviour
    {
        private static readonly int ActivatedParameterHash = Animator.StringToHash("activated");

        public Action eventActivated;

        [SerializeField] private Animator animator;

        [SerializeField] private Button button;

        private bool _activated;

        public bool Activated => _activated;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Activate();
        }

        public void Activate()
        {
            if (!_activated)
            {
                _activated = true;
                animator.SetBool(ActivatedParameterHash, _activated);
                
                eventActivated?.Invoke();
            }
        }

        public void Reset()
        {
            _activated = false;
            animator.SetBool(ActivatedParameterHash, _activated);
        }
    }
}