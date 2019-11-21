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
    public int slotIndex;

    bool isEmptySlot;

    public Type type;

    void Start()
    {
        Init();
    }

    void Init()
    {
        isEmptySlot = slotIndex < 0 || slotIndex >= itemArray.Length;
        if(dragedImage != null)
            Destroy(dragedImage.gameObject);
        if (!isEmptySlot)
        {
            dragedImage = Instantiate(dragedImagePrefab, transform.position, transform.rotation);
            dragedImage.transform.SetParent(transform);
            dragedImage.sprite = itemArray[slotIndex].icon;
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

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragedImage != null)
        {
            dragedImage.transform.SetParent(transform.parent.parent);
            dragedImage.transform.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ItemSlot targetSlot = Inventory.GetInventory().moveItemTargetSlot;
        if (targetSlot != null && (targetSlot.type == Type.ETC || targetSlot.type == itemArray[slotIndex].type))
        {
            int temp = Inventory.GetInventory().moveItemTargetSlot.slotIndex;
            Inventory.GetInventory().moveItemTargetSlot.slotIndex = slotIndex;
            slotIndex = temp;

            Inventory.GetInventory().moveItemTargetSlot.Init();
            Init();
        } else
        {
            dragedImage.transform.SetParent(transform);
            dragedImage.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}
