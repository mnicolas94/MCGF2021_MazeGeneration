using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PopUpOnTrigger : MonoBehaviour
{
    private static readonly int ShowingStateHash = Animator.StringToHash("showing");
    
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
        animator.SetBool(ShowingStateHash, true);
    }

    private void PopDown()
    {
        animator.SetBool(ShowingStateHash, false);
    }
}
