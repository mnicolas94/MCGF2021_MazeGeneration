using System;
using Audio;
using UnityEngine;
using Utils;

namespace Puzzles.Musical
{
    public class MusicalTile : MonoBehaviour
    {
        public Action<Note> eventPressed;
        
        [SerializeField] private Note note;
        [SerializeField] private LayerMask playerMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (playerMask.IsLayerInMask(otherLayer))
            {
                eventPressed?.Invoke(note);
            }
        }
    }
}