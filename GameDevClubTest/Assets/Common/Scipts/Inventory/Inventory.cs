using Assets.Common.Scipts.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform grid;
    public Button inventoryButton;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool IsEnabled = false;
    private void Awake()
    {
        inventoryButton.onClick.AddListener(UseInventoryOnClick);
    }
    private void Start()
    {
        LoadSlots();
    }

    private void LoadSlots()
    {
        for (int i = 0; i < grid.childCount; i++)
        {
            InventorySlot inventorySlot = grid.GetChild(i).GetComponent<InventorySlot>();
            if (inventorySlot != null)
            {
                slots.Add(inventorySlot);

            }
        }
    }

    void UseInventoryOnClick()
    {
        IsEnabled = !IsEnabled;
        inventoryPanel.SetActive(IsEnabled);
    }
}
