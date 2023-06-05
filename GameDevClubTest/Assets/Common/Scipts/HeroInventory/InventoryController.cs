using Assets.Common.Scipts.HeroInventory;
using System.Collections.Generic;
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

    private void Awake()
    {
        LoadSlots();
    }
    public void Subscribe()
    {
        slotChangeWindow.OnUndoSlotButtonChange += OnClickUndoSlotButtonChange;
        slotChangeWindow.OnSlotButtonRemove += OnClickSlotButtonRemove;
    }
    void Unsubscribe()
    {
        slotChangeWindow.OnUndoSlotButtonChange -= OnClickUndoSlotButtonChange;
        slotChangeWindow.OnSlotButtonRemove -= OnClickSlotButtonRemove;
    }
    public void LoadSlots()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
        var CloseInventary = m_Root.Q<Button>();
        CloseInventary.clicked += () => { m_Root.style.display = DisplayStyle.None; ; };

        var item0 = new Item() { ImageName = "5.45x39", typeSlot = TypeSlot.Bullet };
        
        slots.Add(new InventorySlotVE(this));
        var SlotBullet = slots[0];
        SlotBullet.HoldItem(item0, 30);
        m_SlotContainer.Add(SlotBullet);

        for (int i = 0; i < 19; i++)
        {
            InventorySlotVE item = new InventorySlotVE(this);
            slots.Add(item);
            m_SlotContainer.Add(item);
        }
    }
    public void SlotChange(InventorySlotVE inventorySlotVE)
    {
        SelectedInventorySlotVE = inventorySlotVE;
        m_Root.style.display = DisplayStyle.None;
        slotChangeWindow.gameObject.SetActive(true);
        Subscribe();
        slotChangeWindow.SlotChange(inventorySlotVE);
    }

    private void OnClickSlotButtonRemove()
    {
        m_SlotContainer.Remove(SelectedInventorySlotVE);
        slots.Remove(SelectedInventorySlotVE);

        InventorySlotVE item = new InventorySlotVE(this);
        slots.Add(item);
        m_SlotContainer.Add(item);


        m_Root.style.display = DisplayStyle.Flex;
        Unsubscribe();
    }
    private void OnClickUndoSlotButtonChange()
    {
        m_Root.style.display = DisplayStyle.Flex;
        Unsubscribe();

    }
}
