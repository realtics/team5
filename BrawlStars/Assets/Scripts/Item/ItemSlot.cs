using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image dragedImagePrefab;
    Image dragedImage;

    public Item[] itemList;
    public int slotIndex;

    bool isEmptySlot;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        isEmptySlot = slotIndex < 0 || slotIndex >= itemList.Length;
        if (!isEmptySlot)
        {
            dragedImage = Instantiate(dragedImagePrefab, transform.position, transform.rotation);
            dragedImage.transform.SetParent(transform);
            dragedImage.sprite = itemList[slotIndex].icon;
        } else
        {
            if(dragedImage != null)
                Destroy(dragedImage.gameObject);
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
        if (!isEmptySlot)
        {
            dragedImage.transform.SetParent(transform.parent);
            dragedImage.transform.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragedImage.transform.SetParent(transform);
        dragedImage.transform.position = transform.position;

        if(Inventory.GetInventory().moveItemTargetSlot != null)
        {
            int temp = Inventory.GetInventory().moveItemTargetSlot.slotIndex;
            Inventory.GetInventory().moveItemTargetSlot.slotIndex = slotIndex;
            slotIndex = temp;

            Debug.Log(slotIndex);
            Inventory.GetInventory().moveItemTargetSlot.Init();
            Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
