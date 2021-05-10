using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class DataReseter : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> objectsToReset;
        
        private void Awake()
        {
            
        }
    }
}