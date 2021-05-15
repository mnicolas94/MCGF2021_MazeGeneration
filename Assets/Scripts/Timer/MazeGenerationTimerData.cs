using System;
using UnityEngine;

namespace Timer
{
    [CreateAssetMenu(fileName = "TimerData", menuName = "Timer/TimerData", order = 0)]
    public class MazeGenerationTimerData : ResetableScriptableObject
    {
        [SerializeField] private float generateMazeCooldown;
        private float _generateMazeCooldownRuntime;

        public float GenerateMazeCooldown => _generateMazeCooldownRuntime;

        public void SetCooldown(float cooldown)
        {
            _generateMazeCooldownRuntime = cooldown;
        }

        public override void ResetData()
        {
            _generateMazeCooldownRuntime = generateMazeCooldown;
        }
    }
}