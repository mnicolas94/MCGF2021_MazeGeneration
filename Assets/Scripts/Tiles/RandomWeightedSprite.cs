using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Tiles
{
    public class RandomWeightedSprite : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private List<SpriteWeight> sprites;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Start()
        {
            var sprite = GetRandomSprite();
            spriteRenderer.sprite = sprite;
        }

        private Sprite GetRandomSprite()
        {
            float sum = 0;
            foreach (var spriteWeight in sprites)
            {
                sum += spriteWeight.weight;
            }

            float currentValue = 0;
            float randomValue = Random.value;
            foreach (var spriteWeight in sprites)
            {
                float normalizedValue = spriteWeight.weight / sum;
                float min = currentValue;
                float max = currentValue + normalizedValue;
                if (min <= randomValue && randomValue < max)
                {
                    return spriteWeight.sprite;
                }
                currentValue += normalizedValue;
            }

            return defaultSprite;
        }
    }

    [Serializable]
    public struct SpriteWeight
    {
        public Sprite sprite;
        public float weight;
    }
}