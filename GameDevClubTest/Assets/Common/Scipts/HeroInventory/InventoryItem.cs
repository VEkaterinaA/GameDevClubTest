using UnityEngine;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventoryItem : MonoBehaviour
    {
        public Item item;
        public int Count;
        private void Awake()
        {
            item = new Item();
        }
        //public string ItemName;
        //public int Count;
        //public string PathImage;
        //public TypeSlot typeSlot;

    }
}
