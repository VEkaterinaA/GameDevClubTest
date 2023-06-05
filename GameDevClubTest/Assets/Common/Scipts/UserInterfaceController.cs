using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Assets.Common.Scipts
{
    public class UserInterfaceController : MonoBehaviour
    {
        public Button inventoryButton;
        public Text TextBulletCount;
        public InventoryController Inventory;


        private void Awake()
        {
            Inventory.m_Root.style.display = DisplayStyle.None;
            inventoryButton.onClick.AddListener(UseInventoryOnClick);
        }
        void UseInventoryOnClick()
        {
            Inventory.m_Root.style.display = DisplayStyle.Flex;
        }

    }
}
