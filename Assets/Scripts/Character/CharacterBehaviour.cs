using System;
using Character.Data;
using UnityEngine;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterData runtimeCharacterData;
        [SerializeField] private Health characterHealth;

        public CharacterData CharacterData => runtimeCharacterData;

        public Health CharacterHealth => characterHealth;
    }
}