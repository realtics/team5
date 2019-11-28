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

    int equippedSlotIndex = -1;
    bool isDragged;

    void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        if(dragedImage != null)
            Destroy(dragedImage.gameObject);

        if(equippedSlotIndex >= 0)
            item = GameManager.GetInstance().GetEquippedItem(equippedSlotIndex);
        else 
            item = GameManager.GetInstance().GetItemInInventory(itemIndex);

        if (item != null)
        {
            dragedImage = Instantiate(dragedImagePrefab, transform.position, transform.rotation);
            dragedImage.transform.SetParent(transform);
            dragedImage.sprite = item.icon;
        }
    }

    public void SetSlotIndex(int index)
    {
        equippedSlotIndex = index;
    }

    public bool IsEquipSlot()
    {
        return equippedSlotIndex >= 0;
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
        if (dragedImage != null)
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
            if(targetSlot != null && targetSlot != this)
            {
                if (targetSlot.equippedSlotIndex < 0 && equippedSlotIndex < 0)
                    GameManager.GetInstance().SwapSlot(targetSlot.itemIndex, itemIndex);
                else if(targetSlot.equippedSlotIndex >= 0 && targetSlot.type == item.type)
                    GameManager.GetInstance().EquipItem(targetSlot.equippedSlotIndex, itemIndex);
                else if(equippedSlotIndex >= 0 && (targetSlot.item == null || type == targetSlot.item.type))
                    GameManager.GetInstance().EquipItem(equippedSlotIndex, targetSlot.itemIndex);

                Refresh();
                targetSlot.Refresh();
            } else if (dragedImage != null)
            {
                dragedImage.transform.SetParent(transform);
                dragedImage.transform.position = transform.position;
            }
        } else if(itemWindow != null)
        {
            itemWindow.gameObject.SetActive(true);
            itemText.text = item.GetItemExplanation();
        }
    }
}
