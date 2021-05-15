using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class DataReseter : MonoBehaviour
    {
        [SerializeField] private List<ResetableScriptableObject> resetables;
        
        private void Awake()
        {
            foreach (var resetable in resetables)
            {
                resetable.ResetData();
            }
        }
    }
}