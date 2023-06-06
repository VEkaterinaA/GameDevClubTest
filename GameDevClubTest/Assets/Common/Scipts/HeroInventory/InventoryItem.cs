using UnityEngine;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventoryItem : MonoBehaviour
    {
        [HideInInspector]
        public Item item;
        [HideInInspector]
        public int Count;
        public void SetItem(string imahePath, TypeItem typeItem, int count)
        {
            item = new Item(imahePath, typeItem);
            Count = count;
        }
        public void SetItem(Item item, int count)
        {
            this.item = new Item(item.imagePath, item.typeSlot);
            Count = count;
        }

    }
}
