using System;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    [Serializable]
    public class Item
    {
        public string imagePath;
        public TypeItem typeItem;

        public Item(string imagePath, TypeItem typeItem)
        {
            this.imagePath = imagePath;
            this.typeItem = typeItem;
        }
    }

}
