using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    [Serializable]
    public class InventorySlotVisualElement : VisualElement
    {
        public Item item;
        public Image slotImage;
        public Label slotLabelCount;
        public int Count;
        [NonSerialized]
        public int MaxCount;
        public bool IsEmpty;
        private InventoryController _inventoryController;

        public InventorySlotVisualElement(InventoryController inventoryController)
        {

            _inventoryController = inventoryController;

            RegisterCallback<PointerDownEvent>(OnPointerDown);

            AddToClassList("slotContainer");
            IsEmpty = true;
        }
        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || IsEmpty == true)
            {
                return;
            }
            _inventoryController.OpenSlotWindow(this);
        }

        public void SetItem(Item _item, int count)
        {
            AddToClassList("slotClick");

            slotImage = new Image();
            Add(slotImage);
            slotImage.AddToClassList("slotIcon");
            var sprite = Resources.Load<Sprite>(_item.imagePath);
            slotImage.sprite = sprite;
            item = new Item(_item.imagePath, _item.typeItem);

            SetMaxCount();
            SetCount(count);
            IsEmpty = false;
        }
        public void SetCount(int externalCount)
        {
            int sum = Count + externalCount;
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
                if (slotLabelCount == null)
                {
                    slotLabelCount = new Label();
                    slotLabelCount.text = Count.ToString();
                    Add(slotLabelCount);
                    slotLabelCount.AddToClassList("slotLabel");
                }
                else
                {
                    slotLabelCount.text = Count.ToString();
                }
            }

        }
        public InventorySlotVisualElement SubstractCount(int externalCount)
        {
            var sub = Count - externalCount;
            if (sub > 1)
            {
                Count = sub;
                slotLabelCount.text = Count.ToString();
            }
            else if (sub == 1)
            {
                Count = sub;
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
            switch (item.typeItem)
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
}
