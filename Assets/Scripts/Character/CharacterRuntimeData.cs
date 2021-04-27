using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterRuntimeData", menuName = "Character/CharacterRuntimeData", order = 0)]
    public class CharacterRuntimeData : ScriptableObject
    {
        private Vector3 _characterPosition;
        
        [SerializeField] private float lineOfSightRadius;
        [SerializeField] private Vector3 characterPositionOffset;

        public Vector3 CharacterPosition => _characterPosition;

        public float LineOfSightRadius => lineOfSightRadius;

        public Vector3 CharacterPositionOffset => characterPositionOffset;

        public void SetCharacterPosition(Vector3 position)
        {
            _characterPosition = position;
        }

        public void SetLineOfSightRadius(float los)
        {
            lineOfSightRadius = los;
        }
    }
}