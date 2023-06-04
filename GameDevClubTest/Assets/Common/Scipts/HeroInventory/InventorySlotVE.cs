using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventorySlotVE : VisualElement
    {
        public Item item;
        public int MaxCount;
        public bool IsEmpty;
        public InventorySlotVE()
        {
            item = new Item();
            item.image = new Image();
            Add(item.image);
            item.image.AddToClassList("slotIcon");
            AddToClassList("slotContainer");

            IsEmpty = true;
        }

        public void HoldItem(Item _item)
        {
            item.image = _item.image;
            item.GUID = _item.GUID;
            item.typeSlot = _item.typeSlot;
            SetMaxCount();
            SetCount(_item.Count);
            IsEmpty = false;
        }

        public void SetCount(int externalCount)
        {
            if (externalCount >= MaxCount)
            {
                item.Count = externalCount;
            }
            else
            {
                item.Count = MaxCount;
            }
        }
        public void SetMaxCount()
        {
            switch (item.typeSlot)
            {
                case TypeSlot.Weapon:
                    MaxCount = 200;
                    break;
                case TypeSlot.Food:
                    MaxCount = 20;
                    break;
                case TypeSlot.Clothes:
                    MaxCount = 5;
                    break;
                default:
                    MaxCount = 10;
                    break;
            }

        }

    }
}
