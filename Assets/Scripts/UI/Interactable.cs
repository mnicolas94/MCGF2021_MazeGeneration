using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace UI
{
    public class Interactable : MonoBehaviour
    {
        public UnityEvent eventInteracted;
        public UnityEvent eventLeavedInteractionArea;
        
        [SerializeField] private LayerMask characterMask;
        [SerializeField] private Canvas interactableCanvas;
        [SerializeField] private string interactionAxis;

        private bool IsShowing => interactableCanvas.gameObject.activeSelf;

        public void ShowCanvas()
        {
            interactableCanvas.gameObject.SetActive(true);
        }

        public void HideCanvas()
        {
            interactableCanvas.gameObject.SetActive(false);
        }

        private void Start()
        {
            HideCanvas();
        }

        private void Update()
        {
            bool pressedAxis = Input.GetAxisRaw(interactionAxis) > 0.01;
            if (pressedAxis && IsShowing)
            {
                eventInteracted?.Invoke();
                HideCanvas();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (characterMask.IsLayerInMask(otherLayer))
            {
                ShowCanvas();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (characterMask.IsLayerInMask(otherLayer))
            {
                HideCanvas();
                eventLeavedInteractionArea?.Invoke();
            }
        }
    }
}