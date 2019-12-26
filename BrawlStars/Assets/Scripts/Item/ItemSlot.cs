using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image icon;

    public ItemType type;
    public Item item;

    int itemIndex;
	public Text countText;
    Image itemWindow;
    Text itemText;
	Button reinforceButton;

    public bool isEquippedSlot;
	public int equippedIndex;
    bool isDragged;

	public void Init(int index, Image window, Text _itemText, Button button)
	{
		itemIndex = index;
		itemWindow = window;
		itemText = _itemText;
		reinforceButton = button;
	}

    void Start()
    {
		Refresh();

		GameManager.GetInstance().RefreshSlots += Refresh;
	}

    void Refresh()
    {
		if (isEquippedSlot)
			item = GameManager.GetInstance().GetEquippedItem(equippedIndex);
		else
			item = GameManager.GetInstance().GetItemInInventory(itemIndex);

		countText.text = "";
		if (item != null)
		{
			icon.sprite = item.icon;
			icon.transform.localScale = new Vector3(1, 1, 1);

			if (item.type == ItemType.ETC && item.GetCount() > 1)
				countText.text = item.GetCount().ToString();
		} else
		{
			icon.sprite = null;
			icon.transform.localScale = new Vector3(0, 0, 0);
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventory.GetInventory().moveItemTargetSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory.GetInventory().moveItemTargetSlot = null;
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
			ItemSlot targetSlot = Inventory.GetInventory().moveItemTargetSlot;
			if (targetSlot != null && targetSlot != this)
			{
				if (!targetSlot.isEquippedSlot && !isEquippedSlot)
					GameManager.GetInstance().SwapSlot(targetSlot.itemIndex, itemIndex);
				else if (targetSlot.isEquippedSlot && targetSlot.type == item.type)
					GameManager.GetInstance().EquipItem(targetSlot.equippedIndex, itemIndex);
				else if (isEquippedSlot && (targetSlot.item == null || type == targetSlot.item.type))
					GameManager.GetInstance().EquipItem(equippedIndex, targetSlot.itemIndex);

				targetSlot.Refresh();
			}

			icon.transform.SetParent(transform);
			icon.transform.position = transform.position;
			Refresh();
		}
		else if (itemWindow != null)
		{
			itemWindow.gameObject.SetActive(true);
			itemText.text = item.GetItemExplanation();
			if (item.type == ItemType.ETC)
				reinforceButton.gameObject.SetActive(false);
			else
			{
				reinforceButton.gameObject.SetActive(true);
				reinforceButton.onClick.RemoveAllListeners();
				reinforceButton.onClick.AddListener(Reinforce);
			}
		}
    }

	void Reinforce()
	{
		if(GameManager.GetInstance().ReinforceSuccess(item.itemCode, itemIndex, out int materialIndex))
		{
			itemText.text = item.GetItemExplanation();
		}
	}

	void OnDestroy()
	{
		GameManager.GetInstance().RefreshSlots -= Refresh;
	}
}
