using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	Inventory inventory;

	public Image icon;
	public Text countText;

	public ItemType itemType;
    public Item item;

    int itemIndex;
    Image itemWindow;
    Text itemNameText;
	Text itemTooltipText;
	Button reinforceButton;
	Button breakButton;

	public SlotType slotType;
    bool isDragged;

	public void Init(Inventory _inventory, int index, Image window, Text _itemNameText, Text _itemTooltipText, Button _reinforceButton, Button _breakButton)
	{
		inventory = _inventory;
		itemIndex = index;
		itemWindow = window;
		itemNameText = _itemNameText;
		itemTooltipText = _itemTooltipText;
		reinforceButton = _reinforceButton;
		breakButton = _breakButton;
		slotType = SlotType.Normal;
		itemType = ItemType.ETC;
	}

    void Start()
    {
		Refresh();

		GameManager.GetInstance().RefreshSlots += Refresh;
	}

    void Refresh()
    {
		if (slotType == SlotType.Equip)
			item = GameManager.GetInstance().GetEquippedItem(itemIndex);
		else
			item = GameManager.GetInstance().GetItemInInventory(itemIndex);

		countText.text = "";
		if (item != null)
		{
			icon.sprite = item.icon;
			icon.transform.localScale = new Vector3(1, 1, 1);

			if (item.type == ItemType.POTION || item.type == ItemType.ETC)
				countText.text = item.ValueToString();
		} else
		{
			icon.sprite = null;
			icon.transform.localScale = new Vector3(0, 0, 0);
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory.moveItemTargetSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		inventory.moveItemTargetSlot = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragged = true;
        if (item != null)
        {
			icon.transform.SetParent(transform.parent.parent);
			icon.transform.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null)
            return;

		if (isDragged)
		{
			ItemSlot targetSlot = inventory.moveItemTargetSlot;
			if (targetSlot != null && targetSlot != this)
			{
				if (targetSlot.slotType == SlotType.Normal && slotType == SlotType.Normal)
					GameManager.GetInstance().SwapSlot(targetSlot.itemIndex, itemIndex);
				else if (targetSlot.slotType == SlotType.Equip && targetSlot.itemType == item.type)
					GameManager.GetInstance().EquipItem(targetSlot.itemIndex, itemIndex);
				else if (slotType == SlotType.Equip && (targetSlot.item == null || itemType == targetSlot.item.type))
					GameManager.GetInstance().EquipItem(itemIndex, targetSlot.itemIndex);

				targetSlot.Refresh();
			}

			icon.transform.SetParent(transform);
			icon.transform.position = transform.position;
			Refresh();
		}
		else if (itemWindow != null)
		{
			itemWindow.gameObject.SetActive(true);
			SetTooltipText();
			if (item.type == ItemType.ETC) {
				reinforceButton.gameObject.SetActive(false);
				breakButton.gameObject.SetActive(false);
			}
			else
			{
				reinforceButton.gameObject.SetActive(true);
				reinforceButton.onClick.RemoveAllListeners();
				reinforceButton.onClick.AddListener(Reinforce);

				breakButton.gameObject.SetActive(true);
				breakButton.onClick.RemoveAllListeners();
				breakButton.onClick.AddListener(Break);
			}
		}
    }

	void Reinforce()
	{
		bool reinforceSuccess = GameManager.GetInstance().Reinforce(item.itemCode, itemIndex);
		if (reinforceSuccess)
			SetTooltipText();
	}

	void Break()
	{
		bool breakSuccess = GameManager.GetInstance().Break(itemIndex);
		if (breakSuccess)
		{
			if (item == null)
				itemWindow.gameObject.SetActive(false);
			else
				SetTooltipText();
		}
	}

	void SetTooltipText()
	{
		item.GetItemExplanation(ref itemNameText, ref itemTooltipText);
	}

	void OnDestroy()
	{
		GameManager.GetInstance().RefreshSlots -= Refresh;
	}
}
