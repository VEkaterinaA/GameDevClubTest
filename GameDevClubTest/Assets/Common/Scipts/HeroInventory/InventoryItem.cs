using UnityEngine;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventoryItem : MonoBehaviour
    {
        public Item item;
        public int Count;
        public void SetItem(string imahePath, TypeItem typeItem, int count)
        {
            item = new Item(imahePath, typeItem);
            Count = count;
        }
        public void SetItem(Item item, int count)
        {
            item = new Item(item.imagePath, item.typeSlot);
            Count = count;
        }

    }
}
