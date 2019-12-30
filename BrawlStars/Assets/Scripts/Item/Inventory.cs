using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image slotPrefab;
    public ItemSlot moveItemTargetSlot;

    public int row;
    public int column;

    public int horizontalPadding;
    public int verticalPadding;
    public int gap;

    public ItemSlot[] equippedSlot;

    public Image itemWindow;
    public Text itemNameText;
    public Text itemTooltipText;
	public Button reinforceButton;
    public Button breakButton;

    private void Awake()
    {
		GameManager.GetInstance().InitInventory(row, column);
        GameManager.GetInstance().InitEquipSlot(equippedSlot.Length);

		for (int i = 0; i < equippedSlot.Length; i++)
		{
			equippedSlot[i].Init(this, i, itemWindow, itemNameText, itemTooltipText, reinforceButton, breakButton);
		}
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
				slotComponent.Init(this, i * column + j, itemWindow, itemNameText, itemTooltipText, reinforceButton, breakButton);
				slotComponent.SetNormalSlot();
            }
        }

		itemWindow.gameObject.SetActive(false);
    }

	public void ClearInventory()
	{
		GameManager.GetInstance().ClearInventory();
	}
}
