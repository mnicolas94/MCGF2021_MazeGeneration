using System;
using Timer;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelInfoUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI timeText;

        [SerializeField] private MazeGenerationTimer timer;

        private void Start()
        {
            GameManager.Instance.eventNewLevelStarted.AddListener(OnNewLevelStarted);
            HideTime();
        }

        private void Update()
        {
            if (timeText.gameObject.activeSelf)
            {
                UpdateTime();
            }
        }

        private void OnNewLevelStarted()
        {
            levelNumberText.text = $"Lvl {GameManager.Instance.CurrentLevel + 1}";
        }

        public void ShowTime()
        {
            timeText.gameObject.SetActive(true);
        }

        public void HideTime()
        {
            timeText.gameObject.SetActive(false);
        }

        private void UpdateTime()
        {
            if (timer.TimerStoped)
            {
                timeText.text = "-:--";
            }
            else
            {
                float timeLeft = timer.TimeLeft();

                
                int minutes = (int) timeLeft / 60;
                int seconds = (int) timeLeft % 60;
                timeText.text = $"{minutes}:{seconds:00}";
            }
        }
    }
}