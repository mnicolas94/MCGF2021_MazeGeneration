using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemBehaviour : MonoBehaviour
    {
        private Item _item;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI uiText;

        public void SetItem(Item item)
        {
            _item = item;
            UpdateUi();
        }

        public void UpdateUi()
        {
            image.sprite = _item.UiSprite;
            uiText.text = _item.UiText;
        }
    }
}