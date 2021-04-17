using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PopUpOnTrigger : MonoBehaviour
{
    private static readonly int HidenStateHash = Animator.StringToHash("hiden");
    
    [SerializeField] private LayerMask mask;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;
        if (mask.IsLayerInMask(otherLayer)){
            PopUp();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;
        if (mask.IsLayerInMask(otherLayer)){
            PopDown();
        }
    }

    private void PopUp()
    {
        animator.SetBool(HidenStateHash, false);
    }

    private void PopDown()
    {
        animator.SetBool(HidenStateHash, true);
    }
}
