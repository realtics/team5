using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image dragedImagePrefab;
    Image dragedImage;

    public Type type;
    public Item item;
    public int itemIndex;
    public Image itemWindow;
    public Text itemText;

    public bool isEquippedSlot;
    bool isDragged;

    void Start()
    {
		dragedImage = Instantiate(dragedImagePrefab, transform.position, transform.rotation);
		dragedImage.transform.SetParent(transform);

		Refresh();

		GameManager.GetInstance().RefreshSlots += Refresh;
	}

    void Refresh()
    {
        if(isEquippedSlot)
            item = GameManager.GetInstance().GetEquippedItem(itemIndex);
        else
            item = GameManager.GetInstance().GetItemInInventory(itemIndex);

        if (item != null)
		{
			dragedImage.sprite = item.icon;
			dragedImage.transform.localScale = new Vector3(1, 1, 1);
		} else
		{
			dragedImage.sprite = null;
			dragedImage.transform.localScale = new Vector3(0, 0, 0);
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
            dragedImage.transform.SetParent(transform.parent.parent);
            dragedImage.transform.position = eventData.position;
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
					GameManager.GetInstance().EquipItem(targetSlot.itemIndex, itemIndex);
				else if (isEquippedSlot && (targetSlot.item == null || type == targetSlot.item.type))
					GameManager.GetInstance().EquipItem(itemIndex, targetSlot.itemIndex);

				targetSlot.Refresh();
			}

			dragedImage.transform.SetParent(transform);
			dragedImage.transform.position = transform.position;
			Refresh();
		} else if(itemWindow != null)
        {
            itemWindow.gameObject.SetActive(true);
            itemText.text = item.GetItemExplanation();
        }
    }

	void OnDestroy()
	{
		GameManager.GetInstance().RefreshSlots -= Refresh;
	}
}
