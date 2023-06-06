using System;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    [Serializable]
    public class Item
    {
        public string imagePath;
        public TypeItem typeSlot;

        public Item(string imageName, TypeItem typeSlot)
        {
            this.imagePath = imageName;
            this.typeSlot = typeSlot;
        }
    }

}
