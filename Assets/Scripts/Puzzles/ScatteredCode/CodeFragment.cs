using System;
using UnityEngine;
using Utils;

namespace Puzzles.ScatteredCode
{
    public class CodeFragment : MonoBehaviour
    {
        public Action<CodeFragment> eventPickedUp;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        [SerializeField] private float timeToDestroy;

        [SerializeField] private LayerMask playerMask;

        private bool _markedToDestroy;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (!_markedToDestroy && playerMask.IsLayerInMask(otherLayer))
            {
                audioSource.PlayOneShot(clip);
                eventPickedUp?.Invoke(this);
                Destroy(spriteRenderer);
                Destroy(gameObject, timeToDestroy);
                _markedToDestroy = true;
            }
        }
    }
}