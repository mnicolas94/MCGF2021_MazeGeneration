using Character.Data;
using UnityEngine;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;
        [SerializeField] private Health characterHealth;

        public CharacterData CharacterData => characterData;

        public Health CharacterHealth => characterHealth;
    }
}