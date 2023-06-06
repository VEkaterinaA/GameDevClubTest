using System;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    public class Item
    {
        public string imagePath;
        public TypeItem typeSlot;

        public Item(string imagePath, TypeItem typeSlot)
        {
            this.imagePath = imagePath;
            this.typeSlot = typeSlot;
        }
    }

}
