using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MovementTypeUi : MonoBehaviour
    {
        [SerializeField] private MovementOptions optionsData;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private List<TypeText> typesText;
        
        private List<MovementOptions.MovementType> _types;

        private void Start()
        {
            dropdown.onValueChanged.AddListener(OnDropdownSelected);
        
            _types = new List<MovementOptions.MovementType>
            {
                MovementOptions.MovementType.Cartesian,
                MovementOptions.MovementType.IsometricUpLeft,
                MovementOptions.MovementType.IsometricUpRight,
                MovementOptions.MovementType.Hybrid
            };
            
            PopulateOptions();
            dropdown.value = IndexOfType(optionsData.movementType);
        }

        private void OnDropdownSelected(int index)
        {
            optionsData.movementType = _types[index];
        }

        private void PopulateOptions()
        {
            var options = new List<string>();
            foreach (var type in _types)
            {
                string txt = GetUiText(type);
                options.Add(txt);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }

        private int IndexOfType(MovementOptions.MovementType type)
        {
            for (int i = 0; i < _types.Count; i++)
            {
                var t = _types[i];
                if (t == type)
                {
                    return i;
                }
            }

            return -1;
        }

        private string GetUiText(MovementOptions.MovementType type)
        {
            foreach (var tt in typesText)
            {
                if (tt.movementType == type)
                {
                    return tt.uiText;
                }
            }

            return "<text missing>";
        }
    }

    [Serializable]
    public struct TypeText
    {
        public MovementOptions.MovementType movementType;
        public string uiText;
    }
}