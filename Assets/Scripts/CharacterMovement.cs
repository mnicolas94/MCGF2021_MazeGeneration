using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform sprite;
    
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
//        if (dir.magnitude > 0.001)
//            sprite.up = dir;
    }
}
