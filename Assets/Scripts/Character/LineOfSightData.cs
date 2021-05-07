using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "LineOfSightData", menuName = "LoS/LineOfSightData", order = 0)]
    public class LineOfSightData : ScriptableObject
    {
        private Vector3 _characterPosition;
        
        [SerializeField] private float lineOfSightRadius;
        [SerializeField] private float horizontalScale;
        [SerializeField] private Vector3 lineOfSightOffset;
        
        [SerializeField] private float animDuration;
        [SerializeField] private AnimationCurve popOutCurve;
        [SerializeField] private AnimationCurve popInCurve;
        
        public Vector3 CharacterPosition => _characterPosition;

        public float LineOfSightRadius => lineOfSightRadius;

        public Vector3 LineOfSightOffset => lineOfSightOffset;

        public float HorizontalScale => horizontalScale;

        public float AnimDuration => animDuration;

        public AnimationCurve PopOutCurve => popOutCurve;

        public AnimationCurve PopInCurve => popInCurve;

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