using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Character
{
    [CreateAssetMenu(fileName = "LineOfSightData", menuName = "LoS/LineOfSightData", order = 0)]
    public class LineOfSightData : ResetableScriptableObject
    {
        private Vector3 _characterPosition;
        
        [SerializeField] private float lineOfSightRadius;
        private float _maxLineOfSightRadius;
        private float _lineOfSightRadiusRuntime;
        [SerializeField] private float horizontalScale;
        [SerializeField] private Vector3 lineOfSightOffset;
        
        [SerializeField] private float animDuration;
        [SerializeField] private AnimationCurve popOutCurve;
        [SerializeField] private AnimationCurve popInCurve;
        
        public Vector3 CharacterPosition => _characterPosition;

        public float LineOfSightRadius => _lineOfSightRadiusRuntime;
        
        public float MaxLineOfSightRadius => _maxLineOfSightRadius;

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
            _lineOfSightRadiusRuntime = los;
        }
        
        public void SetMaxLineOfSightRadius(float los)
        {
            _maxLineOfSightRadius = los;
        }

        public override void ResetData()
        {
            _maxLineOfSightRadius = lineOfSightRadius;
            _lineOfSightRadiusRuntime = lineOfSightRadius;
        }

        public bool IsInsideRadius(Vector3 position)
        {
            float radius = LineOfSightRadius;
            float sqrRadius = radius * radius;
            var characterOffsetedPosition = CharacterPosition + LineOfSightOffset;
            var toTarget = characterOffsetedPosition - position;
            toTarget.y *= HorizontalScale;  // isometric circle, cartesian ellipsis
            float sqrDist = toTarget.x * toTarget.x + toTarget.y * toTarget.y;
            bool inside = sqrDist < sqrRadius;

            return inside;
        }
    }
}