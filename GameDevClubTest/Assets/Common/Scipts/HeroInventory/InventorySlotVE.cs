using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventorySlotVE : VisualElement
    {
        public Item item;
        public Image slotImage;
        public Label slotLabelCount;
        public int Count;
        public int MaxCount;
        public bool IsEmpty;
        private InventoryController _inventoryController;

        public InventorySlotVE(InventoryController inventoryController)
        {

            _inventoryController = inventoryController;

            RegisterCallback<PointerDownEvent>(OnPointerDown);

            AddToClassList("slotContainer");
            IsEmpty = true;
        }
        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || IsEmpty==true)
            {
                return;
            }
            _inventoryController.SlotChange(this);
        }

        public void HoldItem(Item _item, int count)
        {
            item = new Item();
            AddToClassList("slotClick");

            slotImage = new Image();
            Add(slotImage);
            slotImage.AddToClassList("slotIcon");

            slotImage.sprite = LoadSpriteFromResources.LoadSpriteFromResourcesByName(_item.ImageName);
            item.GUID = _item.GUID;
            item.typeSlot = _item.typeSlot;
            SetMaxCount();
            SetCount(count);
            IsEmpty = false;
        }
        public void SetCount(int externalCount)
        {
            var sum = Count + externalCount;
            if (sum < MaxCount)
            {
                Count = sum;
            }
            else
            {
                Count = MaxCount;
            }
            if (Count > 1)
            {
                slotLabelCount = new Label();
                slotLabelCount.text = Count.ToString();
                Add(slotLabelCount);
                slotLabelCount.AddToClassList("slotLabel");
            }

        }
        public void SetMaxCount()
        {
            switch (item.typeSlot)
            {
                case TypeSlot.Weapon:
                    MaxCount = 3;
                    break;
                case TypeSlot.Food:
                    MaxCount = 20;
                    break;
                case TypeSlot.Clothes:
                    MaxCount = 5;
                    break;
                case TypeSlot.Bullet:
                    MaxCount = 200;
                    break;
                default:
                    MaxCount = 10;
                    break;
            }

        }

    }
    public static class LoadSpriteFromResources
    {
        public static string pathSpritesInventory = "Sprites/Inventory/";
        public static Sprite LoadSpriteFromResourcesByName(string ImageName)
        {
            return Resources.Load<Sprite>(pathSpritesInventory + ImageName); ;
        }
    }

}
