using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemSelectionsUi : MonoBehaviour
    {
        [SerializeField] private RectTransform itemsContainer;
        [SerializeField] private ItemSelectionElementUi itemPrefab;
        [SerializeField] private ShowHidePanel panel;

        [SerializeField] private List<Item> items;
        [SerializeField] private Inventory inventory;
        [SerializeField] private int itemsToSelect;

        private bool _selectingItems;

        public bool SelectingItems => _selectingItems;

        public void ShowItemSelectionPanel()
        {
            _selectingItems = true;
            panel.ShowPanel();
            PopulatePanel();
        }

        public void HideItemSelectionPanel()
        {
            panel.HidePanel();
            _selectingItems = false;
        }

        private void PopulatePanel()
        {
            ClearPanel();
            var availableItems = GetAvailableItems();
            var randomItems = GetRandomItems(availableItems, itemsToSelect);

            foreach (var item in randomItems)
            {
                var itemUi = Instantiate(itemPrefab, itemsContainer);
                itemUi.SetItem(item);
                itemUi.eventItemClicked += AddItemToInventory;
            }
        }

        private void AddItemToInventory(Item item)
        {
            inventory.AddItem(item);
            HideItemSelectionPanel();
        }

        private void ClearPanel()
        {
            var children = itemsContainer.GetComponentsInChildren<ItemSelectionElementUi>();
            foreach (var child in children)
            {
                Destroy(child.gameObject);
            }
        }

        private List<Item> GetAvailableItems()
        {
            var availableItems = new List<Item>();
            foreach (var item in items)
            {
                bool alreadyInInventory = inventory.Items.Contains(item);
                if (!alreadyInInventory)
                {
                    availableItems.Add(item);
                }
            }

            return availableItems;
        }

        private List<Item> GetRandomItems(List<Item> availableItems, int count)
        {
            var randomItems = new List<Item>();
            while (count > 0 && availableItems.Count > 0)
            {
                int index = Random.Range(0, availableItems.Count);
                var item = availableItems[index];
                availableItems.RemoveAt(index);
                randomItems.Add(item);
                
                count--;
            }

            return randomItems;
        }
    }
}