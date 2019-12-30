﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static Inventory instance = null;

    public Image slotPrefab;
    public ItemSlot moveItemTargetSlot;

    public int row;
    public int column;

    public int horizontalPadding;
    public int verticalPadding;
    public int gap;

    public Image itemWindow;
    public Text itemText;
	public Button reinforceButton;

    private void Awake()
    {
        instance = this;

		GameManager.GetInstance().InitInventory(row, column);
	}

    public static Inventory GetInventory()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector2 position;
        moveItemTargetSlot = null;

		for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                Image slot = Instantiate(slotPrefab);
                slot.transform.SetParent(transform);
				slot.transform.localScale = new Vector3(1, 1, 1);

                position.x = verticalPadding + (slotPrefab.rectTransform.sizeDelta.x + gap) * j + slotPrefab.rectTransform.sizeDelta.x / 2;
                position.y = -(horizontalPadding + (slotPrefab.rectTransform.sizeDelta.y + gap) * i + slotPrefab.rectTransform.sizeDelta.y / 2);

                slot.rectTransform.anchoredPosition = position;

				ItemSlot slotComponent = slot.GetComponent<ItemSlot>();
				slotComponent.Init(i * column + j, itemWindow, itemText, reinforceButton);
            }
        }

		itemWindow.gameObject.SetActive(false);
    }

	public void ClearInventory()
	{
		GameManager.GetInstance().ClearInventory();
	}
}