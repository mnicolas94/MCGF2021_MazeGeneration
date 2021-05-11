using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Puzzles.Musical
{
    public class MusicalPuzzlePanel : MonoBehaviour
    {
        [SerializeField] private PuzzleData puzzle;
        
        [Space]
        
        [SerializeField] private List<NoteFrequency> frequencies;
        [SerializeField] private List<NoteSprite> sprites;

        [SerializeField] private RectTransform notesContainer;
        [SerializeField] private Image notePrefab;
        [SerializeField] private int notesSequenceLength;

        [SerializeField] private SendAudioSignalFilter signalFilter;
        
        private List<MusicalTile> _tiles;
        private List<Note> _generatedSequence;
        private List<Note> _currentPressedSequence;

        private void Start()
        {
            var tiles = FindObjectsOfType<MusicalTile>();
            _tiles = new List<MusicalTile>(tiles);
            foreach (var tile in tiles)
            {
                tile.eventPressed += OnTilePressed;
            }
            _generatedSequence = new List<Note>();
            _currentPressedSequence = new List<Note>();
            GenerateMusic();
            UpdateUi();
        }

        private void GenerateMusic()
        {
            _generatedSequence.Clear();
            
            int notesLeft = notesSequenceLength;
            while (notesLeft > 0)
            {
                int index = Random.Range(0, sprites.Count);

                var noteSprite = sprites[index];
                
                _generatedSequence.Add(noteSprite.note);
                
                notesLeft--;
            }
        }

        private void UpdateUi()
        {
            ClearUi();
            foreach (var note in _generatedSequence)
            {
                var sprite = GetSpriteByNote(note);
                var image = Instantiate(notePrefab, notesContainer);
                image.sprite = sprite;
            }
        }

        private void ClearUi()
        {
            var children = notesContainer.GetComponentsInChildren<Image>();
            foreach (var item in children)
            {
                Destroy(item.gameObject);
            }
        }

        private void OnTilePressed(Note note)
        {
            float freq = GetFrequencyByNote(note);
            signalFilter.sineFrequency = freq;
            signalFilter.Send();
            
            _currentPressedSequence.Add(note);
            HandlePressedSequenceSize();
            
            bool solved = CheckPuzzleSolved();
            if (solved)
            {
                GameManager.Instance.NotifyPuzzleSolved(puzzle);
            }
        }

        private bool CheckPuzzleSolved()
        {
            if (_generatedSequence.Count != _currentPressedSequence.Count)
            {
                return false;
            }
            
            for (int i = 0; i < _generatedSequence.Count; i++)
            {
                if (_generatedSequence[i] != _currentPressedSequence[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void HandlePressedSequenceSize()
        {
            while (_currentPressedSequence.Count > notesSequenceLength)
            {
                _currentPressedSequence.RemoveAt(0);
            }
        }

        private float GetFrequencyByNote(Note note)
        {
            foreach (var frequency in frequencies)
            {
                if (frequency.note == note)
                {
                    return frequency.freq;
                }
            }

            return 0.0f;
        }
        
        private Sprite GetSpriteByNote(Note note)
        {
            foreach (var noteSprite in sprites)
            {
                if (noteSprite.note == note)
                {
                    return noteSprite.sprite;
                }
            }

            return sprites[0].sprite;
        }
    }

    public enum Note
    {
        C, D, E, F, G, A, B
    }
    
    [Serializable]
    public struct NoteFrequency
    {
        public Note note;
        public float freq;
    }
    
    [Serializable]
    public struct NoteSprite
    {
        public Note note;
        public Sprite sprite;
    }
}