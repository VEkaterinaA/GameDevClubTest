using Assets.Common.Scipts.HeroInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using Item = Assets.Common.Scipts.HeroInventory.Item;

public class InventoryController : MonoBehaviour
{
    public SlotChangeWindow slotChangeWindow;
    [HideInInspector]
    public VisualElement m_Root;
    private VisualElement m_SlotContainer;

    private InventorySlotVE SelectedInventorySlotVE;

    [HideInInspector]
    public List<InventorySlotVE> slots = new List<InventorySlotVE>();
    public event Action<int> OnDropBullet;
    public int CountBullet;
    private void Awake()
    {
        LoadSlotComponents();
    }
    private void Start()
    {
        CreateSlots();
    }
    public void Subscribe()
    {
        slotChangeWindow.OnUndoSlotButtonChange += UndoSlotButtonChangeOnClick;
        slotChangeWindow.OnSlotButtonRemove += RemoveSlotButtonOnClick;
    }
    void Unsubscribe()
    {
        slotChangeWindow.OnUndoSlotButtonChange -= UndoSlotButtonChangeOnClick;
        slotChangeWindow.OnSlotButtonRemove -= RemoveSlotButtonOnClick;
    }
    public void LoadSlotComponents()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
        var CloseInventary = m_Root.Q<Button>();
        CloseInventary.clicked += () => { m_Root.style.display = DisplayStyle.None; ; };
    }
    public void SlotChange(InventorySlotVE inventorySlotVE)
    {
        SelectedInventorySlotVE = inventorySlotVE;
        m_Root.style.display = DisplayStyle.None;
        slotChangeWindow.gameObject.SetActive(true);
        Subscribe();
        slotChangeWindow.SlotChange(inventorySlotVE);
    }

    private void RemoveSlotButtonOnClick()
    {
        RemoveSlot(SelectedInventorySlotVE);

        m_Root.style.display = DisplayStyle.Flex;
        Unsubscribe();
    }
    private void UndoSlotButtonChangeOnClick()
    {
        m_Root.style.display = DisplayStyle.Flex;
        Unsubscribe();

    }
    public void SpendItem(Item item,int count)
    {
        var slot = slots.FirstOrDefault(u=>u.item==item);
        if(slot==null)
        {
            Debug.Log("There is no such object in the inventory");
            return;
        }
        if(slot.item.typeSlot==TypeItem.Bullet)
        {
            CountBullet -= count;
            OnDropBullet?.Invoke(CountBullet);

        }
        var slotSub = slot.SubstractCount(count);
        if(slotSub ==null)
        {
            return;
        }
        RemoveSlot(slotSub);
    }
    private void RemoveSlot(InventorySlotVE inventorySlotVE)
    {
        m_SlotContainer.Remove(inventorySlotVE);
        slots.Remove(inventorySlotVE);

        InventorySlotVE item = new InventorySlotVE(this);
        slots.Add(item);
        m_SlotContainer.Add(item);

    }
    private void CreateSlots()
    {
        var itemBullet = new Item("Sprites/Inventory/Bullet/5.45x39", TypeItem.Bullet);
        CountBullet = 30;
        OnDropBullet?.Invoke(CountBullet);
        slots.Add(new InventorySlotVE(this));
        var SlotBullet = slots[0];
        SlotBullet.HoldItem(itemBullet, 30);
        m_SlotContainer.Add(SlotBullet);

        for (int i = 0; i < 19; i++)
        {
            InventorySlotVE item = new InventorySlotVE(this);
            slots.Add(item);
            m_SlotContainer.Add(item);
        }

    }
}
