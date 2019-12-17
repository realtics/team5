using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    public Character player;
    Dictionary<string, Item> itemTable;
    public Item[] itemTableElements;
    string[] InventoryItemNameArray;
    string[] equippedItemNameArray;
	[SerializeField]
	DropTable dropTable = null;

	Status itemStatus;
	[HideInInspector]
	public int stageIndex;
	public Action RefreshSlots;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
	}

    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemTable = new Dictionary<string, Item>();
        for(int i = 0; i < itemTableElements.Length; i++)
        {
            itemTable.Add(itemTableElements[i].itemCode, itemTableElements[i]);
		}
		dropTable.Init();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
    }

    public void InitInventory(int row, int column)
    {
		InventoryItemNameArray = new string[row * column];
		for (int i = 0; i < InventoryItemNameArray.Length; i++)
		{
			InventoryItemNameArray[i] = PlayerPrefs.GetString("inventory" + i, "");
		}

	}

	public void InitEquipSlot(int count)
	{
		equippedItemNameArray = new string[count];
		for (int i = 0; i < count; i++)
		{
			equippedItemNameArray[i] = PlayerPrefs.GetString("equip" + i, "");
		}
	}

	public void ClearInventory()
	{
		for (int i = 0; i < InventoryItemNameArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("inventory" + i);
			SetSlot(i);
		}
		for (int i = 0; i < equippedItemNameArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("equip" + i);
			SetEquipSlot(i);
		}
		RefreshSlots();
	}

    public bool AddNewItemInInventory(string itemName)
    {
		int emptySlotIndex = FindMinimumEmptySlotIndex();
        
        if(emptySlotIndex >= 0)
        {
			SetSlot(emptySlotIndex, itemName);
            return true;
        } else
        {
            return false;
        }
    }

	int FindMinimumEmptySlotIndex()
	{
		int emptySlotIndex;
		for (emptySlotIndex = 0; emptySlotIndex < InventoryItemNameArray.Length; emptySlotIndex++)
		{
			if (InventoryItemNameArray[emptySlotIndex] == "")
				return emptySlotIndex;
		}
		return -1;
	}

    public Item GetItemInInventory(int index)
    {
        if (InventoryItemNameArray[index] == "")
            return null;
        else
            return itemTable[InventoryItemNameArray[index]];
    }

    public Item GetEquippedItem(int index)
    {
        if (equippedItemNameArray[index] == "")
            return null;
        else
            return itemTable[equippedItemNameArray[index]];
    }

    public void SwapSlot(int index1, int index2)
	{
		string item1 = equippedItemNameArray[index1];
		string item2 = InventoryItemNameArray[index2];
		SetSlot(index1, item2);
		SetSlot(index2, item1);
    }

    public void EquipItem(int equippedIndex, int originalIndex)
    {
		string equippedItem = equippedItemNameArray[equippedIndex];
		string originalItem = InventoryItemNameArray[originalIndex];
		SetEquipSlot(equippedIndex, originalItem);
		SetSlot(originalIndex, equippedItem);

        itemStatus.attackDamage = 0;
        itemStatus.armor = 0;
        itemStatus.hp = 0;
        itemStatus.hpRecovery = 0;
        itemStatus.moveSpeed = 0;

        for (int i = 0; i < equippedItemNameArray.Length; i++)
        {
            if (equippedItemNameArray[i] != "")
            {
                itemStatus += itemTable[equippedItemNameArray[i]].status;
            }
        }
    }

	void SetSlot(int index, string newItemName = "")
	{
		InventoryItemNameArray[index] = newItemName;
		PlayerPrefs.SetString("inventory" + index, newItemName);
	}

	void SetEquipSlot(int index, string newItemName = "")
	{
		equippedItemNameArray[index] = newItemName;
		PlayerPrefs.SetString("equip" + index, newItemName);
	}

    public Status GetFinalStatus()
    {
        return player.status + itemStatus;
    }

    public string GetPlayerInfoString()
    {
        Status finalStatus = player.status + itemStatus;
        string content = "공격력 : " + finalStatus.attackDamage + " (" + player.status.attackDamage + " + " + itemStatus.attackDamage + ")\n";
        content += "방어력 : " + finalStatus.armor + " (" + player.status.armor + " + " + itemStatus.armor + ")\n";
        content += "체력 : " + finalStatus.hp + " (" + player.status.hp + " + " + itemStatus.hp + ")\n";
        content += "체력회복 : " + finalStatus.hpRecovery + " (" + player.status.hpRecovery + " + " + itemStatus.hpRecovery + ")\n";
        content += "이동속도 : " + finalStatus.moveSpeed + " (" + player.status.moveSpeed + " + " + itemStatus.moveSpeed + ")\n";
        return content;
    }

	public DropItem[] GetDropItemList(string monsterName)
	{
		return dropTable.GetDropItemList(monsterName);
	}

	public Item GetItem(string itemName)
	{
		return itemTable[itemName];
	}
}