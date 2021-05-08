using System;
using UnityEngine;

namespace Items
{
    public class InventoryBehaviour : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private RectTransform itemsContainer;

        [SerializeField] private ItemBehaviour itemBehaviourPrefab;

        private void Awake()
        {
            inventory.Clear();
        }

        private void Start()
        {
            inventory.eventItemAdded += AddItemToUi;
            inventory.eventItemRemoved += RemoveItemToUi;
            inventory.eventItemUpdated += UpdateUi;
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            Clear();
            foreach (var item in inventory.Items)
            {
                var itemBehaviour = Instantiate(itemBehaviourPrefab, itemsContainer);
                itemBehaviour.SetItem(item);
            }
        }
        
        private void UpdateUi(Item item)
        {
            UpdateUi();
        }

        private void AddItemToUi(Item item)
        {
            UpdateUi();
        }
        
        private void RemoveItemToUi(Item item)
        {
            UpdateUi();
        }

        private void Clear()
        {
            var children = itemsContainer.GetComponentsInChildren<ItemBehaviour>();
            foreach (var item in children)
            {
                Destroy(item.gameObject);
            }
        }
    }
}