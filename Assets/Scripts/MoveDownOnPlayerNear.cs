using Character;
using UnityEngine;

public class MoveDownOnPlayerNear : MonoBehaviour
{
    [SerializeField] private CharacterRuntimeData characterRuntimeData;
    
    [Space]
    
    [SerializeField] private float radius;
    [SerializeField] private Vector2 playerPositionOffset;
    [SerializeField] private float maxVerticalDisplacement;

    private Vector3 _initialPosition;
    
    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        HandleDistanceToPlayer();
    }

    private void HandleDistanceToPlayer()
    {
        Transform transf = transform;
        var position = transf.position;
        float sqrRadius = radius * radius;
        var toTarget = characterRuntimeData.CharacterPosition + (Vector3) playerPositionOffset - position;
        float sqrDist = toTarget.x * toTarget.x + toTarget.y * toTarget.y;
        float radDiff = Mathf.Max(0, sqrRadius - sqrDist);
        float radRatio = radDiff / sqrRadius;
        radRatio = Mathf.Clamp01(radRatio);
        float verticalDisplacement = radRatio * maxVerticalDisplacement;

        position.y = _initialPosition.y - verticalDisplacement;
        transf.position = position;
    }
}