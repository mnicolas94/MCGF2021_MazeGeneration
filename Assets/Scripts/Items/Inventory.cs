using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Items/Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        public Action<Item> eventItemAdded;
        public Action<Item> eventItemRemoved;
        public Action<Item> eventItemUpdated;
        
        [SerializeField] private List<Item> items;

        public List<Item> Items => new List<Item>(items);  // returns a copy

        public void ClearEvents()
        {
            eventItemAdded = null;
            eventItemRemoved = null;
            eventItemUpdated = null;
        }

        public bool HasItem(Item item)
        {
            return items.Contains(item);
        }
        
        public void AddItem(Item item)
        {
            bool alreadyInInventory = HasItem(item);
            if (!alreadyInInventory)
            {
                if (!item.Consumable)
                {
                    items.Add(item);
                    eventItemAdded?.Invoke(item);
                }
                item.ApplyEffectOnPickUp();
            }
        }
        
        public void RemoveItem(Item item)
        {
            bool inInventory = HasItem(item);
            if (inInventory)
            {
                items.Remove(item);
                eventItemRemoved?.Invoke(item);
            }
        }

        public void UpdateItem(Item item)
        {
            bool inInventory = HasItem(item);
            if (inInventory)
            {
                eventItemUpdated?.Invoke(item);
            }
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}