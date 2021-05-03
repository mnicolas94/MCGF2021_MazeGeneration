using System.Collections.Generic;
using UnityEngine;

public class UpdateShadersPlayerPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private List<Material> materials;

    private void Update()
    {
        foreach (var material in materials)
        {
            material.SetVector("_PlayerPos", target.transform.position);
        }
    }
}