using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Utils;

public class PopUpOnPlayerNear : MonoBehaviour
{
    [SerializeField] private CharacterRuntimeData characterRuntimeData;
    
    [Space]
    
    [SerializeField] private float animDuration;
    [SerializeField] private AnimationCurve popOutCurve;
    [SerializeField] private AnimationCurve popInCurve;
    
    private bool _inside;
    private float _timeSinceLastChange;
    
    private void FixedUpdate()
    {
        HandleDistanceToPlayer();
        SetScale();
    }

    private void HandleDistanceToPlayer()
    {
        float radius = characterRuntimeData.LineOfSightRadius;
        float sqrRadius = radius * radius;
        var characterOffsetedPosition = characterRuntimeData.CharacterPosition + characterRuntimeData.LineOfSightOffset;
        var toTarget = characterOffsetedPosition - transform.position;
        toTarget.y *= characterRuntimeData.HorizontalScale;  // isometric circle, cartesian ellipsis
        float sqrDist = toTarget.x * toTarget.x + toTarget.y * toTarget.y;
        bool inside = sqrDist < sqrRadius;
        bool toggle = inside ^ _inside;
        if (toggle)
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        _inside = !_inside;
        _timeSinceLastChange = Time.time;
    }

    private void SetScale()
    {
        float timeDiff = Time.time - _timeSinceLastChange;
        float normTime = timeDiff / animDuration;
        normTime = Mathf.Clamp01(normTime);

        var curve = _inside ? popOutCurve : popInCurve;
        float value = curve.Evaluate(normTime);
        
        var scale = transform.localScale;
        scale.x = value;
        scale.y = value;
        transform.localScale = scale;
    }
}
