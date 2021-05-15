using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private string characterName;
        [SerializeField] private Color nameColor;
        [SerializeField] private CharacterStats statistics;
        [SerializeField] private CharacterBattlesData battlesData;

        public string CharacterName => characterName;

        public CharacterStats Statistics => statistics;

        public CharacterBattlesData BattlesData => battlesData;
        
        public Color NameColor => nameColor;
    }
}