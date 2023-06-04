using System;
using UnityEngine.UIElements;

namespace Assets.Common.Scipts.HeroInventory
{
    [Serializable]
    public class Item
    {
        public string GUID;
        public Image image;
        public int Count;
        public TypeSlot typeSlot;


    }

}
