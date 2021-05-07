using System;
using System.Collections.Generic;
using Puzzles.LeversPuzzle;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzles.Combinations
{
    public class CombinationsPanel : MonoBehaviour
    {
        [SerializeField] private Interactable interactable;
        [SerializeField] private ShowHidePanel panel;

        [SerializeField] private CombinationButton button1;
        [SerializeField] private CombinationButton button2;
        [SerializeField] private CombinationButton button3;

        private Dictionary<Vector3Int, CombinationLever> _levers;
        private List<int> _buttonsPressOrder;
        private List<int> _pulledLeversOrder;
        
        private void Start()
        {
            interactable.eventInteracted.AddListener(OnInteraction);
            _buttonsPressOrder = new List<int>();

            button1.eventActivated += () => OnButtonPressed(1);
            button2.eventActivated += () => OnButtonPressed(2);
            button3.eventActivated += () => OnButtonPressed(3);
            
            InitializeLeversDictionary();
            _pulledLeversOrder = new List<int>();
        }
        
        private void InitializeLeversDictionary()
        {
            var levers = FindObjectsOfType<CombinationLever>();
            _levers = new Dictionary<Vector3Int, CombinationLever>();

            var combinations = new []
            {
                new Vector3Int(1, 2, 3),
                new Vector3Int(1, 3, 2),
                new Vector3Int(2, 1, 3),
                new Vector3Int(2, 3, 1),
                new Vector3Int(3, 1, 2),
                new Vector3Int(3, 2, 1),
            };

            var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
            
            for (int i = 0; i < 6; i++)
            {
                var lever = levers[i];
                var combination = combinations[i];
                _levers.Add(combination, lever);
                lever.eventLeverStateChanged += () => OnLeverStateChanged(lever);

                int index = Random.Range(0, numbers.Count);
                int number = numbers[index];
                numbers.RemoveAt(index);
                lever.SetNumber(number);
            }
        }

        private void OnInteraction()
        {
            _buttonsPressOrder.Clear();
            ResetButtons();
        }

        private void OnButtonPressed(int index)
        {
            _buttonsPressOrder.Add(index);

            if (_buttonsPressOrder.Count == 3)  // all pressed
            {
                Invoke(nameof(OnAllButtonsPressed), 0.3f);
            }
        }

        private void OnAllButtonsPressed()
        {
            var combination = new Vector3Int(
                _buttonsPressOrder[0],
                _buttonsPressOrder[1],
                _buttonsPressOrder[2]
            );
            CloseAllLevers();
            OpenLever(combination);
            panel.HidePanel();
        }

        private void ResetButtons()
        {
            button1.Reset();
            button2.Reset();
            button3.Reset();
        }

        private void OpenLever(Vector3Int combination)
        {
            _levers[combination].Open();
        }

        private void CloseAllLevers()
        {
            foreach (var combination in _levers.Keys)
            {
                var lever = _levers[combination];
                lever.Close();
            }
        }

        private void CloseAllLeversPanels()
        {
            foreach (var combination in _levers.Keys)
            {
                var lever = _levers[combination];
                lever.ClosePanel();
            }
        }

        private void OnLeverStateChanged(CombinationLever combinationLever)
        {
            int number = combinationLever.Number;
            
            if (combinationLever.Activated)
            {
                _pulledLeversOrder.Add(number);
                HandleLeversOrderSize();
                if (IsPuzzleSolved())
                {
                    Invoke(nameof(NotifyPuzzleSolved), 0.2f);
                }
            }
            else
            {
                int index = _buttonsPressOrder.FindIndex((x) => x == number);
                if (index > 0)
                {
                    _buttonsPressOrder.RemoveAt(index);
                }
            }
        }

        private void HandleLeversOrderSize()
        {
            while (_pulledLeversOrder.Count > _levers.Count)
            {
                _pulledLeversOrder.RemoveAt(0);
            }
        }

        private bool IsPuzzleSolved()
        {
            bool countIsRight = _pulledLeversOrder.Count == _levers.Count;

            if (!countIsRight)
            {
                return false;
            }
            
            for (int i = 1; i < _pulledLeversOrder.Count; i++)
            {
                int aboveValue = _pulledLeversOrder[i - 1];
                int currentValue = _pulledLeversOrder[i];
                if (aboveValue >= currentValue)
                {
                    return false;
                }
            }

            return true;
        }

        private void NotifyPuzzleSolved()
        {
            CloseAllLeversPanels();
            GameManager.Instance.NotifyPuzzleSolved();
        }
    }
}