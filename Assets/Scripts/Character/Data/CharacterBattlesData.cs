using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "CharacterBattlesData", menuName = "Characters/CharacterBattlesData", order = 0)]
    public class CharacterBattlesData : ScriptableObject
    {
        [SerializeField] private Sprite battleSprite;
        [SerializeField] private Color nameColor;

        [Space]
        
        [SerializeField] private List<string> startBattleDialogues;
        [SerializeField] private List<AttackChance> attacks;

        public Sprite BattleSprite => battleSprite;

        public Color NameColor => nameColor;

        public string GetRandomStartDialogue()
        {
            int index = Random.Range(0, startBattleDialogues.Count);
            return startBattleDialogues[index];
        }

        public CharacterAttack GetRandomAttack()
        {
            var accumulatedChances = new List<float>();

            float sum = 0;
            foreach (var attackChance in attacks)
            {
                sum += attackChance.chanceWeight;
                accumulatedChances.Add(sum);
            }

            float randomValue = Random.value;
            for (int i = 0; i < accumulatedChances.Count; i++)
            {
                float accumulatedChance = accumulatedChances[i];
                float accumulatedNormalizedChance = accumulatedChance / sum;
                if (randomValue <= accumulatedNormalizedChance)
                {
                    return attacks[i].attack;
                }
            }
            
            return attacks[0].attack;
        }
    }

    [Serializable]
    public struct AttackChance
    {
        public float chanceWeight;
        public CharacterAttack attack;
    }
}