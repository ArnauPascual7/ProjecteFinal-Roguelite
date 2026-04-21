using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<InventorySlot> Container = new List<InventorySlot>();

        public void OnCollisionEnter2D(Collision2D collision)
        {
            var item = collision.gameObject.GetComponent<Item>();
            if (item)
            {
                AddItem(item.item);
                Destroy(collision.gameObject);
            }
        }

        public void AddItem(ItemObject _item)
        {
            Container.Add(new InventorySlot(_item));
        }

        [Serializable]
        public class InventorySlot
        {
            public ItemObject Item;
            public InventorySlot(ItemObject item)
            {
                this.Item = item;
            }

        }
    }
}