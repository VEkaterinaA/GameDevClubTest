﻿using System;
using Unity.VisualScripting;
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
            AddToClassList("slotClick");

            slotImage = new Image();
            Add(slotImage);
            slotImage.AddToClassList("slotIcon");

            slotImage.sprite = LoadSpriteFromResources.LoadSpriteFromResourcesByName(_item.imagePath);
            item = new Item(_item.imagePath, _item.typeSlot);

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
        public InventorySlotVE SubstractCount(int externalCount)
        {
            var sub = Count - externalCount;
            if(sub>1)
            {
                Count = sub;
            }
            else if(sub==1)
            {
                Remove(slotLabelCount);
                slotLabelCount = null;
            }
            else
            {
                return this;
            }
            return null;
        }
        public void SetMaxCount()
        {
            switch (item.typeSlot)
            {
                case TypeItem.Weapon:
                    MaxCount = 3;
                    break;
                case TypeItem.Food:
                    MaxCount = 20;
                    break;
                case TypeItem.Clothes:
                    MaxCount = 5;
                    break;
                case TypeItem.Bullet:
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
        public static Sprite LoadSpriteFromResourcesByName(string ImageName)
        {
            return Resources.Load<Sprite>(ImageName); ;
        }
    }

}