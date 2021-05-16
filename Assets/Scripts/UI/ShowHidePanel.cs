using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ShowHidePanel : MonoBehaviour
    {
        public Action eventShowed;
        public Action eventHiden;
        
        [SerializeField] private RectTransform panel;
        [SerializeField] private Canvas canvas;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float upLerpSpeed;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float downLerpSpeed;

        [SerializeField] private float arriveConditionThreshold;

        [SerializeField] private string cancelAxis;
        [SerializeField] private bool cancellable;

        private Vector3 _originalPosition;
        private Vector3 _hidePosition;
        private Vector3 _targetPosition;
        private float _targetLerpSpeed;
        private bool _moving;
        
        private void Start()
        {
            _originalPosition = panel.localPosition;
            float panelHalfWidth = panel.sizeDelta.y / 2;
            float canvasHalfHeight = canvas.renderingDisplaySize.y;
            _hidePosition = _originalPosition - Vector3.up * (canvasHalfHeight + panelHalfWidth + 10);

            panel.localPosition = _hidePosition;
        }

        private void Update()
        {
            if (cancellable)
            {
                if (Input.GetButtonDown(cancelAxis))
                {
                    HidePanel();
                }
            }
        }

        [NaughtyAttributes.Button]
        public void ShowPanel()
        {
            _targetPosition = _originalPosition;
            _targetLerpSpeed = upLerpSpeed;
            if (!_moving)
                StartCoroutine(MoveToTarget(true));
            eventShowed?.Invoke();
        }
        
        [NaughtyAttributes.Button]
        public void HidePanel()
        {
            _targetPosition = _hidePosition;
            _targetLerpSpeed = downLerpSpeed;
            if (!_moving)
                StartCoroutine(MoveToTarget(false));
            eventHiden?.Invoke();
        }
        
        public void HidePanel(float time)
        {
            Invoke(nameof(HidePanel), time);
        }

        private IEnumerator MoveToTarget(bool finalActiveState)
        {
            _moving = true;

            if (finalActiveState)
            {
                panel.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            
            while ((panel.localPosition - _targetPosition).magnitude > arriveConditionThreshold)
            {
                panel.localPosition = Vector3.Lerp(panel.localPosition, _targetPosition, _targetLerpSpeed);
                yield return null;
            }

            panel.localPosition = _targetPosition;
            
            if (!finalActiveState)
            {
                panel.gameObject.SetActive(false);
            }
            
            _moving = false;
        }
    }
}