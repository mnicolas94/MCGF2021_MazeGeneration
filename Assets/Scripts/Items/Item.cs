using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Sprite uiSprite;
        [SerializeField] private string itemName;
        [SerializeField] private string description;

        [Space]
        
        [SerializeField] private string uiText;  // just used for ScatteredCode, maybe another items will use it as well later

        public Sprite UiSprite => uiSprite;

        public string ItemName => itemName;

        public string Description => description;

        public string UiText => uiText;

        public void SetSprite(Sprite sprite)
        {
            uiSprite = sprite;
        }

        public void SetName(string nameOfItem)
        {
            itemName = nameOfItem;
        }

        public void SetDescription(string desc)
        {
            description = desc;
        }

        public void SetUiText(string text)
        {
            uiText = text;
        }
        
        public abstract void ApplyEffectOnPickUp();
    }
}