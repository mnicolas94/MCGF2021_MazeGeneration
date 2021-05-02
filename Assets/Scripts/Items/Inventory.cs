using System;
using System.Collections.Generic;
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
        
        public void AddItem(Item item)
        {
            bool alreadyInInventory = items.Contains(item);
            if (!alreadyInInventory)
            {
                items.Add(item);
                eventItemAdded?.Invoke(item);
            }
        }
        
        public void RemoveItem(Item item)
        {
            bool inInventory = items.Contains(item);
            if (inInventory)
            {
                items.Remove(item);
                eventItemRemoved?.Invoke(item);
            }
        }

        public void UpdateItem(Item item)
        {
            bool inInventory = items.Contains(item);
            if (inInventory)
            {
                eventItemUpdated?.Invoke(item);
            }
        }
    }
}