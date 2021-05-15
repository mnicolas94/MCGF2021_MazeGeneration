using UI;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "ShowTimerUi", menuName = "Items/ShowTimerUi", order = 0)]
    public class ShowTimerUi : Item
    {
        public override void ApplyEffectOnPickUp()
        {
            var timer = FindObjectOfType<LevelInfoUi>();
            timer.ShowTime();
        }
    }
}