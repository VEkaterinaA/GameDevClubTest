using Assets.Common.Scipts.HeroInventory;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Zenject.Asteroids;
using Button = UnityEngine.UI.Button;

public class InventoryController : MonoBehaviour
{
    public Button inventoryButton;

    [HideInInspector]
    public List<InventorySlotVE> slots = new List<InventorySlotVE>();

    private bool IsEnabled = false;

    private VisualElement m_Root;
    private VisualElement m_SlotContainer;

    private string pathSprites = "Sprites/Inventory/";
    private void Start()
    {
        LoadSlots();

        inventoryButton.onClick.AddListener(UseInventoryOnClick);
        slots[4].item.image.sprite = Resources.Load<Sprite>(pathSprites + "5.45x39");
    }

    public void LoadSlots()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");

        for (int i = 0; i < 20; i++)
        {
            InventorySlotVE item = new InventorySlotVE();
            slots.Add(item);
            m_SlotContainer.Add(item);
        }
    }
    void UseInventoryOnClick()
    {
        IsEnabled = !IsEnabled;
        gameObject.SetActive(IsEnabled);
    }

}
