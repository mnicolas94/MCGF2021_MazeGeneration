using Timer;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "TimeRaiser", menuName = "Items/TimeRaiser", order = 0)]
    public class TimeRaiser : Item
    {
        [SerializeField] private MazeGenerationTimerData timerData;
        [SerializeField] private float timeAmount;
        
        public override void ApplyEffectOnPickUp()
        {
            timerData.SetCooldown(timerData.GenerateMazeCooldown + timeAmount);
        }
    }
}