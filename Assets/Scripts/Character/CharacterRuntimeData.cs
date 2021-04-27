using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterRuntimeData", menuName = "Character/CharacterRuntimeData", order = 0)]
    public class CharacterRuntimeData : ScriptableObject
    {
        private Vector3 _characterPosition;

        public Vector3 CharacterPosition => _characterPosition;

        public void SetCharacterPosition(Vector3 position)
        {
            _characterPosition = position;
        }
    }
}