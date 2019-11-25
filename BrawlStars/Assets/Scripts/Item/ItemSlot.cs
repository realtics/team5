using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image dragedImagePrefab;
    Image dragedImage;

    public Item[] itemArray;
    public int itemIndex;

    public Type type;

    public Image itemWindow;
    public Text itemText;

    bool isEmptySlot;
    int equippedSlotIndex = -1;
    bool isDragged;

    void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        isEmptySlot = itemIndex < 0 || itemIndex >= itemArray.Length;
        if(dragedImage != null)
            Destroy(dragedImage.gameObject);

        if (!isEmptySlot)
        {
            dragedImage = Instantiate(dragedImagePrefab, transform.position, transform.rotation);
            dragedImage.transform.SetParent(transform);
            dragedImage.sprite = itemArray[itemIndex].icon;

            if(equippedSlotIndex >= 0)
                GameManager.GetInstance().SetEquippedItem(equippedSlotIndex, itemArray[itemIndex]);
        }
    }

    public void SetSlotIndex(int index)
    {
        equippedSlotIndex = index;
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
        if (isEmptySlot)
            return;

        if (isDragged)
        {
            ItemSlot targetSlot = Inventory.GetInventory().moveItemTargetSlot;
            if (targetSlot != null && (targetSlot.type == Type.ETC || targetSlot.type == itemArray[itemIndex].type))
            {
                int temp = Inventory.GetInventory().moveItemTargetSlot.itemIndex;
                Inventory.GetInventory().moveItemTargetSlot.itemIndex = itemIndex;
                itemIndex = temp;

                Inventory.GetInventory().moveItemTargetSlot.Refresh();
                Refresh();
            }
            else
            {
                dragedImage.transform.SetParent(transform);
                dragedImage.transform.position = transform.position;
            }
        } else if(itemWindow != null)
        {
            itemWindow.gameObject.SetActive(true);
            itemText.text = itemArray[itemIndex].GetItemExplanation();
        }
    }
}
