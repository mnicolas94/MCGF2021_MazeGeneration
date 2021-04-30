using System.Collections;
using UnityEngine;

namespace PuzzlesUI.LeversPuzzle
{
    public class LeversPuzzleLevers : MonoBehaviour
    {
        [SerializeField] private ShowPanelOnInteraction panel;
        
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
            bool codeIsRight = CheckCode();

            if (codeIsRight && _diagram.InteractedAtLeastOnce)
            {
                panel.HidePanel();
                GameManager.Instance.NotifyPuzzleSolved();
            }
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
    }
}