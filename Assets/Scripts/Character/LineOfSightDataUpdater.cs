using System;
using UnityEngine;

namespace Character
{
    public class LineOfSightDataUpdater : MonoBehaviour
    {
        [SerializeField] private LineOfSightData losData;
        [SerializeField] private Transform characterTransform;

        private void FixedUpdate()
        {
            losData.SetCharacterPosition(characterTransform.position);
        }
    }
}