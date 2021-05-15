using Character;
using UnityEngine;

namespace Items.Implementations
{
    [CreateAssetMenu(fileName = "LoSRaiser", menuName = "Items/LoSRaiser", order = 0)]
    public class LosRaiser : Item
    {
        [SerializeField] private LineOfSightData losData;
        [SerializeField] private float losAmount;
        
        public override void ApplyEffectOnPickUp()
        {
            losData.SetMaxLineOfSightRadius(losData.MaxLineOfSightRadius + losAmount);
        }
    }
}