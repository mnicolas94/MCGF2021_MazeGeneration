using TMPro;
using UI;
using UnityEngine;

namespace Puzzles.NumbersPuzzle
{
    public class NumbersPuzzleDiagram : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Interactable interactable;

        private string _generatedSequence;

        public string GeneratedSequence => _generatedSequence;

        private void Start()
        {
            interactable.eventInteracted.AddListener(GenerateCode);
            GenerateCode();
        }

        private void GenerateCode()
        {
            _generatedSequence = GenerateSequence();
            text.text = _generatedSequence;
        }

        private string GenerateSequence()
        {
            int n1 = Random.Range(0, 10);
            int n2 = Random.Range(0, 10);
            int n3 = Random.Range(0, 10);
            int n4 = Random.Range(0, 10);
            int n5 = Random.Range(0, 10);
            int n6 = Random.Range(0, 10);
            int n7 = Random.Range(0, 10);

            return $"{n1}{n2}{n3}{n4}{n5}{n6}{n7}";
        }
    }
}