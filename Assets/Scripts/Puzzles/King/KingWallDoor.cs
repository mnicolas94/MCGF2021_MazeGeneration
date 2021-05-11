using System;
using System.Collections;
using UnityEngine;

namespace Puzzles.King
{
    public class KingWallDoor : MonoBehaviour
    {
        [SerializeField] private Transform moveDownTransform;
        [SerializeField] private Collider2D wallCollider;
        [SerializeField] private MoveDownOnPlayerNear moveDownScript;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite spriteOpened;

        [SerializeField] private Vector3 movement;
        [SerializeField] private float openingDuration;

        private float _startOpeningTime;
        private bool _alreadyOpened;
        
        public void Open(Action onWallOpened)
        {
            if (!_alreadyOpened)
            {
                moveDownScript.enabled = false;
                _alreadyOpened = true;
                StartCoroutine(OpenCoroutine(onWallOpened));
            }
        }

        private IEnumerator OpenCoroutine(Action onWallOpened)
        {
            _startOpeningTime = Time.time;
            var initialPosition = moveDownTransform.position;
            var targetPosition = initialPosition + movement;

            float dist = Vector3.Distance(initialPosition, targetPosition);
            while (dist > 1E-2)
            {
                float elapsedTime = Time.time - _startOpeningTime;
                float normalizedTime = elapsedTime / openingDuration;

                var currentPosition = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);
                moveDownTransform.position = currentPosition;
                
                dist = Vector3.Distance(currentPosition, targetPosition);
                yield return null;
            }

            moveDownTransform.position = initialPosition;
            spriteRenderer.sprite = spriteOpened;
            wallCollider.enabled = false;
            onWallOpened?.Invoke();
        }
    }
}