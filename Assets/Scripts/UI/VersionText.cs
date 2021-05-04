using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            SetText();
        }

        [NaughtyAttributes.Button]
        private void SetText()
        {
            text.text = $"Build {GetProjectVersion()}";
        }
        
        private string GetProjectVersion()
        {
            return Application.version;
        }
    }
}