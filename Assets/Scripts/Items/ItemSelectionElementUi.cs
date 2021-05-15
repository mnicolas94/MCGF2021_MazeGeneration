using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemSelectionElementUi : MonoBehaviour
    {
        public Action<Item> eventItemClicked;
        
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Button button;

        private Item _item;

        public Item Item => _item;

        public void SetItem(Item item)
        {
            _item = item;
            itemImage.sprite = item.UiSprite;
            itemNameText.text = item.ItemName;
            itemDescriptionText.text = item.Description;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.7f);
            GetFocus();
        }

        private void GetFocus()
        {
            button.Select();
            button.onClick.AddListener(NotifyClick);
        }
        
        private void NotifyClick()
        {
            eventItemClicked?.Invoke(_item);
        }
    }
}