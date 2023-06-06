using Assets.Common.Scipts.HeroInventory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Common.Scipts.Hero.HelperClasses
{
    public class HeroCollisionWithObjects
    {
        public void MoveItemToInventory(Collision2D collisionDetails, List<InventorySlotVE> slots)
        {
            var inventoryItem = collisionDetails.gameObject.GetComponent<InventoryItem>();

            if (inventoryItem == null)
            {
                return;
            }
            var item = inventoryItem.item;
            var slot = slots.FirstOrDefault(u =>u.IsEmpty!=true && (u.item.imagePath == item.imagePath && u.item.typeSlot == item.typeSlot));
            if (slot == null)
            {
                var firstEmptySlot = slots.FirstOrDefault(u => u.IsEmpty==true);

                if(firstEmptySlot!=null)
                {
                    firstEmptySlot.HoldItem(item, inventoryItem.Count);
                    return;
                }
                else
                {
                    Debug.Log("Inventory is full!");
                    return;
                }
            }
            else
            {
                slot.SetCount(inventoryItem.Count);
                return;
            }
        }
    }
}
