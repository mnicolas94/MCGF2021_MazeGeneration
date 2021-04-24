﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharacterMovement : MonoBehaviour
{
    public static int SpeedHashId = Animator.StringToHash("speed");
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float speed;
    
    public float Speed => speed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 dir)
    {
        if (!enabled)
            return;
        _rb.velocity = dir.normalized * speed;

        if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
        } else if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Update()
    {
        animator.SetFloat(SpeedHashId, _rb.velocity.magnitude);
    }
}