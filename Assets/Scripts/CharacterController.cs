using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterMovement runner;
    public Material downMaterial;
    
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        runner.Move(new Vector2(h, v));
    }

    private void Update()
    {
        downMaterial.SetVector("_PlayerPos", transform.position);
    }
}
