using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private GameObject creditsPanel;

        private void Start()
        {
            openButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);
            
            Close();
        }

        private void Close()
        {
            creditsPanel.SetActive(false);
        }

        private void Open()
        {
            creditsPanel.SetActive(true);
        }
    }
}