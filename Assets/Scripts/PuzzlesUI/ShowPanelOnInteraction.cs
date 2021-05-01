using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzlesUI
{
    public class ShowPanelOnInteraction : MonoBehaviour
    {
        public Action eventShowed;
        
        [SerializeField] private RectTransform panel;
        [SerializeField] private Canvas canvas;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float lerpSpeed;

        [SerializeField] private string cancelAxis;

        private Vector3 _originalPosition;
        private Vector3 _hidePosition;
        private Vector3 _targetPosition;
        private bool _moving;
        private void Start()
        {
            panel.gameObject.SetActive(true);
            _originalPosition = panel.localPosition;
            float panelHalfWidth = panel.sizeDelta.y / 2;
            float canvasHalfHeight = canvas.renderingDisplaySize.y;
            _hidePosition = _originalPosition - Vector3.up * (canvasHalfHeight + panelHalfWidth + 10);

            panel.localPosition = _hidePosition;
        }

        private void Update()
        {
            if (Input.GetAxisRaw(cancelAxis) > 0E-5)
            {
                HidePanel();
            }
        }

        [NaughtyAttributes.Button]
        public void ShowPanel()
        {
            _targetPosition = _originalPosition;
            if (!_moving)
                StartCoroutine(MoveToTarget());
            eventShowed?.Invoke();
        }
        
        [NaughtyAttributes.Button]
        public void HidePanel()
        {
            _targetPosition = _hidePosition;
            if (!_moving)
                StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            _moving = true;
            while ((panel.localPosition - _targetPosition).magnitude > 0E-3)
            {
                panel.localPosition = Vector3.Lerp(panel.localPosition, _targetPosition, lerpSpeed);
                yield return null;
            }

            panel.localPosition = _targetPosition;
            _moving = false;
        }
    }
}