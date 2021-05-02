using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Puzzles.LeversPuzzle
{
    public class LeversPuzzleDiagram : MonoBehaviour
    {
        [SerializeField] private Interactable interactable;
        
        [SerializeField] private Sprite upPos;
        [SerializeField] private Sprite downPos;

        [SerializeField] private Image pos1;
        [SerializeField] private Image pos2;
        [SerializeField] private Image pos3;
        [SerializeField] private Image pos4;

        private bool _interactedAtLeastOnce;
        public bool InteractedAtLeastOnce => _interactedAtLeastOnce;

        private bool[] _codeGenerated;

        public bool[] CodeGenerated => _codeGenerated;

        private void Start()
        {
            interactable.eventInteracted.AddListener(OnInteraction);
            _codeGenerated = GenerateCode();
            while (AllFalse(_codeGenerated))
            {
                _codeGenerated = GenerateCode();
            }
            SetupUi();
        }

        private void OnInteraction()
        {
            _interactedAtLeastOnce = true;
        }
        
        private void SetupUi()
        {
            pos1.sprite = _codeGenerated[0] ? downPos: upPos;
            pos2.sprite = _codeGenerated[1] ? downPos: upPos;
            pos3.sprite = _codeGenerated[2] ? downPos: upPos;
            pos4.sprite = _codeGenerated[3] ? downPos: upPos;
        }
        
        private bool[] GenerateCode()
        {
            return new []
            {
                Random.Range(0, 2) == 0,
                Random.Range(0, 2) == 0,
                Random.Range(0, 2) == 0,
                Random.Range(0, 2) == 0
            };
        }
        
        private bool AllFalse(bool[] code)
        {
            bool allFalse = true;
            foreach (var c in code)
            {
                allFalse &= !c;
            }
            return allFalse;
        }
    }
}