using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Material downMaterial;

    [SerializeField] private bool isometricUpGoesLeft;
    
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var dir = new Vector2();
        int alt = isometricUpGoesLeft ? 1 : -1;
        dir.x = h - v * alt;
        dir.y = (h * alt + v) / 2;
        
        characterMovement.Move(dir);
    }

    private void Update()
    {
        downMaterial.SetVector("_PlayerPos", transform.position);
    }
}
