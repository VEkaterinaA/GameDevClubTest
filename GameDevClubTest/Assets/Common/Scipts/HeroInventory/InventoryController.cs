using Assets.Common.Scipts;
using Assets.Common.Scipts.HeroInventory;
using Assets.Common.Scipts.Weapon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Item = Assets.Common.Scipts.HeroInventory.Item;

public class InventoryController : MonoBehaviour
{
    private VisualElement SlotContainerVisualElement;
    private InventorySlotVisualElement SelectedInventorySlot;
    private FileOperations _fileOperations;
    public HeroWeapon _heroWeapon;
    public SlotWindow SlotWindowSceneObject;
    public VisualElement InventoryVisualRoot;
    [HideInInspector]
    public List<InventorySlotVisualElement> slots = new List<InventorySlotVisualElement>(20);

    [Inject]
    private void Construct(FileOperations fileOperations, HeroWeapon heroWeapon)
    {
        _fileOperations = fileOperations;
        _heroWeapon = heroWeapon;
    }

    private void Awake()
    {
        LoadInventoryMainWindow();
        LoadSlotContainerComponent();
        InitCloseButton();
    }
    private void Start()
    {
        InitSlots();
    }
    private void InitSubscribtions()
    {
        SlotWindowSceneObject.OnUndoSlotButtonChange += UndoSlotButtonChangeOnClick;
        SlotWindowSceneObject.OnSlotButtonRemove += RemoveSlotButtonOnClick;
    }
    private void Unsubscribe()
    {
        SlotWindowSceneObject.OnUndoSlotButtonChange -= UndoSlotButtonChangeOnClick;
        SlotWindowSceneObject.OnSlotButtonRemove -= RemoveSlotButtonOnClick;
    }
    private void LoadSlotContainerComponent()
    {
        SlotContainerVisualElement = InventoryVisualRoot.Q<VisualElement>("SlotContainer");
    }
    private void LoadInventoryMainWindow()
    {
        InventoryVisualRoot = GetComponent<UIDocument>().rootVisualElement;
    }
    private void InitCloseButton()
    {
        var CloseInventary = InventoryVisualRoot.Q<Button>();
        CloseInventary.clicked += () => { InventoryVisualRoot.style.display = DisplayStyle.None; };
    }

    public void OpenSlotWindow(InventorySlotVisualElement inventorySlotVE)
    {
        SelectedInventorySlot = inventorySlotVE;
        ChangeInventoryWindowDisplayStyle(DisplayStyle.None);
        SlotWindowSceneObject.gameObject.SetActive(true);
        InitSubscribtions();
        SlotWindowSceneObject.ChangeSlot(inventorySlotVE);
    }
    private void ChangeInventoryWindowDisplayStyle(DisplayStyle style)
    {
        InventoryVisualRoot.style.display = style;
    }
    private void RemoveSlotButtonOnClick()
    {
        RemoveSlot(SelectedInventorySlot);

        InventoryVisualRoot.style.display = DisplayStyle.Flex;
        Unsubscribe();
    }
    private void UndoSlotButtonChangeOnClick()
    {
        InventoryVisualRoot.style.display = DisplayStyle.Flex;
        Unsubscribe();

    }
    public void SubstractItem(Item item, int count)
    {
        var slot = slots.FirstOrDefault(u => u.item == item);
        if (slot == null)
        {
            Debug.Log("There is no such object in the inventory");
            return;
        }
        var slotSub = slot.SubstractCount(count);
        if (slotSub == null)
        {
            return;
        }
        RemoveSlot(slotSub);
    }
    private void RemoveSlot(InventorySlotVisualElement inventorySlotVE)
    {
        SlotContainerVisualElement.Remove(inventorySlotVE);
        slots.Remove(inventorySlotVE);

        InventorySlotVisualElement item = new InventorySlotVisualElement(this);
        //slots.Add(item);
        //slots.Sort((u, v) =>
        //{
        //    if (!u.IsEmpty && !v.IsEmpty)//
        //        return 0;

        //    return u.IsEmpty.CompareTo(v.IsEmpty) == -1 ? -1 : 1;
        //});
        SlotContainerVisualElement.Add(item);

    }
    private void InitSlots()
    {
        for (int i = 0; i < 20; i++)
        {
            InventorySlotVisualElement item = new InventorySlotVisualElement(this);
            slots.Add(item);
            SlotContainerVisualElement.Add(item);
        }
        _fileOperations.LoadInventory(slots);
        BulletSlot();
    }

    private void BulletSlot()
    {
        var slotBullet = slots.FirstOrDefault(u => u.item != null && u.item.typeItem == TypeItem.Bullet);
        if (slotBullet == null)
        {
            var emptySlot = slots.FirstOrDefault(u => u.IsEmpty == true);
            if (emptySlot == null)
            {
                return;
            }
            AddBulletSlot(emptySlot);
            return;
        }
        AddBulletSlot(slotBullet);
    }

    private void AddBulletSlot(InventorySlotVisualElement emptySlot)
    {
        emptySlot.SetItem(CreateItem("Sprites/Inventory/Bullet/5.45x39", TypeItem.Bullet), 30);
        _heroWeapon.InitBullet(emptySlot);
    }

    private Item CreateItem(string sprite, TypeItem type)
    {
        return new Item(sprite, type);
    }

    public void PickUpItem(InventoryItem inventoryItem)
    {
        if (inventoryItem == null)
        {
            return;
        }
        var item = inventoryItem.item;
        Debug.Log(item.imagePath + "->" + item.typeItem);
        var slot = slots.FirstOrDefault(u => u.IsEmpty != true && (u.item.imagePath == item.imagePath && u.item.typeItem == item.typeItem));
        if (slot == null)
        {
            var firstEmptySlot = slots.FirstOrDefault(u => u.IsEmpty == true);

            if (firstEmptySlot != null)
            {
                firstEmptySlot.SetItem(item, inventoryItem.Count);
                return;
            }
            else
            {
                Debug.Log("Inventory is full!");
                return;
            }
        }
        else
        {
            slot.SetCount(inventoryItem.Count);
            return;
        }

    }
}
