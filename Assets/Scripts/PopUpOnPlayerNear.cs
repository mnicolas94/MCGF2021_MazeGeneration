using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Utils;

public class PopUpOnPlayerNear : MonoBehaviour
{
    [SerializeField] private LineOfSightData lineOfSightData;
    
    private bool _inside;
    private float _timeSinceLastChange;
    
    private void FixedUpdate()
    {
        HandleDistanceToPlayer();
        SetScale();
    }

    private void HandleDistanceToPlayer()
    {
        float radius = lineOfSightData.LineOfSightRadius;
        float sqrRadius = radius * radius;
        var characterOffsetedPosition = lineOfSightData.CharacterPosition + lineOfSightData.LineOfSightOffset;
        var toTarget = characterOffsetedPosition - transform.position;
        toTarget.y *= lineOfSightData.HorizontalScale;  // isometric circle, cartesian ellipsis
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
        float normTime = timeDiff / lineOfSightData.AnimDuration;
        normTime = Mathf.Clamp01(normTime);

        var curve = _inside ? lineOfSightData.PopOutCurve : lineOfSightData.PopInCurve;
        float value = curve.Evaluate(normTime);
        
        var scale = transform.localScale;
        scale.x = value;
        scale.y = value;
        transform.localScale = scale;
    }
}
