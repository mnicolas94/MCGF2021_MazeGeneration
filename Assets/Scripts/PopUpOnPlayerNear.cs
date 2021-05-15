using Character;
using UnityEngine;

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
        bool inside = lineOfSightData.IsInsideRadius(transform.position);
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
