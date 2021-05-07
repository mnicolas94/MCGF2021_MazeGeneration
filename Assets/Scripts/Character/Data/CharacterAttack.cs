using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "CharacterAttack", menuName = "Characters/Attacks/CharacterAttack", order = 0)]
    public class CharacterAttack : ScriptableObject
    {
        [SerializeField] private string attackName;
        [SerializeField] private int damage;

        public string AttackName => attackName;

        public int Damage => damage;
    }
}