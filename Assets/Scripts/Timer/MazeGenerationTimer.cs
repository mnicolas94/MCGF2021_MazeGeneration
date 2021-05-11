using System;
using UnityEngine;

namespace Timer
{
    public class MazeGenerationTimer : MonoBehaviour
    {
        [SerializeField] private MazeGenerationTimerData timerData;

        [SerializeField] private float timeLeftStartSound;
        [SerializeField] private AudioSource audioSource;

        private bool _timerStoped;
        private float _startTime;
        private bool _soundPlaying;
        
        private void Start()
        {
            GameManager.Instance.eventNewLevelStarted += ResetTimer;
            ResetTimer();
        }

        public void ResetTimer()
        {
            _timerStoped = false;
            _startTime = Time.time;
            _soundPlaying = false;
            audioSource.Stop();
        }

        public void StopTimer()
        {
            ResetTimer();
            _timerStoped = true;
        }

        private void Update()
        {
            if (_timerStoped)
                return;
            
            float timeLeft = TimeLeft();
            if (timeLeft <= timeLeftStartSound && !_soundPlaying)
            {
                audioSource.Play();
                _soundPlaying = true;
            }

            if (timeLeft < 0)
            {
                StopTimer();
                GameManager.Instance.RestartLevel();
            }
        }

        private float TimeLeft()
        {
            return TimeToGenerate() - Time.time;
        }

        private float TimeToGenerate()
        {
            return _startTime + timerData.GenerateMazeCooldown;
        }
    }
}