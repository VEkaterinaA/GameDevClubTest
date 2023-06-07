using Assets.Common.Scipts.HeroInventory;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotWindow : MonoBehaviour
{
    private InventorySlotVisualElement _item;

    public event Action OnUndoSlotButtonChange;
    public event Action OnSlotButtonRemove;

    public Button CloseButton;
    public Button DeleteButton;
    public Image Image;
    public Text Label;

    private void Awake()
    {
        CloseButton.onClick.AddListener(OnClickUndoSlotButtonChange);
        DeleteButton.onClick.AddListener(OnClickSlotButtonRemove);
    }
    private void OnClickUndoSlotButtonChange()
    {
        OnUndoSlotButtonChange?.Invoke();
        gameObject.SetActive(false);
    }
    private void OnClickSlotButtonRemove()
    {
        OnSlotButtonRemove?.Invoke();
        gameObject.SetActive(false);
    }

    public void ChangeSlot(InventorySlotVisualElement inventorySlotVE)
    {
        Image.sprite = inventorySlotVE.slotImage.sprite;
        Image.type = Image.Type.Simple;
        Image.preserveAspect = true;
        if (inventorySlotVE.slotLabelCount != null)
        {
            Label.text = inventorySlotVE.slotLabelCount.text;
        }
        else
        {
            Label.text = "1";
        }
    }
}
