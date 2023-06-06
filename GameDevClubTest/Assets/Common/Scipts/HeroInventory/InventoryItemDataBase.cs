using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scipts.HeroInventory
{
    public class InventoryItemDataBase
    {
        private List<Item> inventoryItems = new List<Item>();

        private string pathClothes = "Sprites/Inventory/Clothes";
        private string pathWeapons = "Sprites/Inventory/Weapons";
        private string pathBullet = "Sprites/Inventory/Bullet";
        public InventoryItemDataBase()
        {
            LoadItem(pathClothes, TypeItem.Clothes);
            LoadItem(pathWeapons, TypeItem.Weapon);
            LoadItem(pathBullet, TypeItem.Bullet);
        }
        private void LoadItem(string path, TypeItem typeItem)
        {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>(path))
            {
                inventoryItems.Add(new Item(path + "/" + sprite.name, typeItem));
            }

        }
        public List<Item> GetInventoryItems()
        {
            return inventoryItems;
        }
        public Item GetRandomInventoryItem()
        {
            int number = Random.Range(0, inventoryItems.Count-1);
            return inventoryItems[number];
        }
    }
}
