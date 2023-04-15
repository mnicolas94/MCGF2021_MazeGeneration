using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class UpdateLoSRadius : MonoBehaviour
{
    public float initialValue;
    public float radius;
    public LineOfSightData data;
    
    void Update()
    {
        data.SetLineOfSightRadius(radius);
    }

    [ContextMenu("Initialize")]
    public void Initialize()
    {
        radius = initialValue;
    }
}
