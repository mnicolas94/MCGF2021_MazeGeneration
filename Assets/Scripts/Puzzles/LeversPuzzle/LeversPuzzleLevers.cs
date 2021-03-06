using System.Collections;
using UI;
using UnityEngine;

namespace Puzzles.LeversPuzzle
{
    public class LeversPuzzleLevers : MonoBehaviour
    {
        [SerializeField] private PuzzleData puzzle;
        
        [Space]
        
        [SerializeField] private ShowHidePanel panel;
        
        [SerializeField] private Lever lever1;
        [SerializeField] private Lever lever2;
        [SerializeField] private Lever lever3;
        [SerializeField] private Lever lever4;
        
        private LeversPuzzleDiagram _diagram;

        private IEnumerator Start()
        {
            lever1.eventLeverStateChanged += OnLeverStateChanged;
            lever2.eventLeverStateChanged += OnLeverStateChanged;
            lever3.eventLeverStateChanged += OnLeverStateChanged;
            lever4.eventLeverStateChanged += OnLeverStateChanged;
            
            yield return null;  // wait one frame

            _diagram = FindObjectOfType<LeversPuzzleDiagram>();  // should exist only one
        }

        private void OnLeverStateChanged()
        {
            if (IsPuzzleSolved())
            {
                DisableAllLevers();
                float delayTime = 0.4f;
                panel.HidePanel(delayTime);
                GameManager.Instance.NotifyPuzzleSolved(puzzle, delayTime);
            }
        }

        private bool IsPuzzleSolved()
        {
            bool codeIsRight = CheckCode();
            return codeIsRight && _diagram.InteractedAtLeastOnce;
        }
        
        private bool CheckCode()
        {
            var code = _diagram.CodeGenerated;

            bool codeIsRight = true;

            codeIsRight &= code[0] == lever1.Activated;
            codeIsRight &= code[1] == lever2.Activated;
            codeIsRight &= code[2] == lever3.Activated;
            codeIsRight &= code[3] == lever4.Activated;

            return codeIsRight;
        }

        public void DisableAllLevers()
        {
            lever1.Disable();
            lever2.Disable();
            lever3.Disable();
            lever4.Disable();
        }

    }
}