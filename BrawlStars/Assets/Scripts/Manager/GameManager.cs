using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	static GameManager instance = null;

	public Character player;
	public ItemTable itemTable;
	Dictionary<int, Item> itemDataBase;
	int itemIndex;
	int[] InventoryItemArray;
	int[] equippedItemArray;
	Status itemStatus;
	public DropTable dropTable = null;

	Dictionary<string, Skill> skillTable;
	public Skill[] skillTableElements;

	[HideInInspector]
	public int stageIndex;
	public Action RefreshSlots;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		instance = this;
		itemIndex = 0;
	}

	public static GameManager GetInstance()
	{
		return instance;
	}

	// Start is called before the first frame update
	void Start()
	{
		itemTable.Init();
		dropTable.Init();

		itemDataBase = new Dictionary<int, Item>();
		for (int i = 0; ; i++)
		{
			string itemCode = PlayerPrefs.GetString("itemDataBase" + i + "name", "");
			if (itemCode == "")
				break;
			itemDataBase.Add(i, itemTable.GetItem(itemCode));
			itemIndex++;
		}

		skillTable = new Dictionary<string, Skill>();
		for (int i = 0; i < skillTableElements.Length; i++)
		{
			skillTableElements[i].MakeTargetRangeMesh();
			skillTable.Add(skillTableElements[i].skillCode, skillTableElements[i]);
		}

		for (int i = 0; i < player.skillCodeArray.Length; i++)
		{
			string skillCode = PlayerPrefs.GetString("Skill" + i, "");
			player.skillCodeArray[i] = skillCode;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	public void InitInventory(int row, int column)
	{
		InventoryItemArray = new int[row * column];
		for (int i = 0; i < InventoryItemArray.Length; i++)
		{
			InventoryItemArray[i] = PlayerPrefs.GetInt("inventory" + i, -1);
		}
	}

	public void InitEquipSlot(int count)
	{
		equippedItemArray = new int[count];
		for (int i = 0; i < count; i++)
		{
			equippedItemArray[i] = PlayerPrefs.GetInt("equip" + i, -1);
		}
		RefreshEquipStatus();
	}

	public void ClearInventory()
	{
		for (int i = 0; i < InventoryItemArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("inventory" + i);
			InventoryItemArray[i] = -1;
		}
		for (int i = 0; i < equippedItemArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("equip" + i);
			equippedItemArray[i] = -1;
		}
		foreach (int i in itemDataBase.Keys)
		{
			PlayerPrefs.DeleteKey("itemDataBase" + i + "name");
			PlayerPrefs.DeleteKey("itemDataBase" + i + "count");
			PlayerPrefs.DeleteKey("itemDataBase" + i + "reinforce");
		}
		itemDataBase.Clear();
		itemIndex = 0;
		RefreshSlots();
	}

	public bool AddNewItemInInventory(string itemCode)
	{
		int possibleSlotIndex = FindMinimumPossibleSlotIndex(itemCode);

		if (possibleSlotIndex >= 0)
		{
			if (InventoryItemArray[possibleSlotIndex] < 0)
			{
				Item newItem = new Item(itemTable.GetItem(itemCode));
				SetSlot(possibleSlotIndex, itemIndex);
				AddNewItemInDataBase(newItem);
			}
			else
			{
				Item newItem = itemDataBase[InventoryItemArray[possibleSlotIndex]];
				newItem.AddOneCount();
				PlayerPrefs.SetInt("itemDataBase" + itemIndex + "count", newItem.GetCount());
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void AddNewItemInDataBase(Item newItem)
	{
		PlayerPrefs.SetString("itemDataBase" + itemIndex + "name", newItem.itemCode);
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "count", 1);
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "reinforce", 0);
		itemDataBase.Add(itemIndex++, newItem);
	}

	int FindMinimumPossibleSlotIndex(string itemCode)
	{
		int sameItemSlotIndex;
		int emptySlotIndex = -1;
		for (sameItemSlotIndex = 0; sameItemSlotIndex < InventoryItemArray.Length; sameItemSlotIndex++)
		{
			if (InventoryItemArray[sameItemSlotIndex] < 0)
			{
				if (emptySlotIndex < 0)
					emptySlotIndex = sameItemSlotIndex;
			}
			else
			{
				Item itemInSlot = itemDataBase[InventoryItemArray[sameItemSlotIndex]];
				if (itemInSlot.type == ItemType.ETC && itemInSlot.itemCode == itemCode)
					return sameItemSlotIndex;
			}
		}
		return emptySlotIndex;
	}

	public Item GetItemInInventory(int index)
	{
		if (InventoryItemArray[index] < 0)
			return null;
		else
			return itemDataBase[InventoryItemArray[index]];
	}

	public bool ReinforceSuccess(string itemCode, int index)
	{
		for (int i = 0; i < InventoryItemArray.Length; i++)
		{
			if (i != index && itemDataBase[InventoryItemArray[i]].itemCode == itemCode)
			{
				itemDataBase.Remove(InventoryItemArray[i]);
				PlayerPrefs.DeleteKey("itemDataBase" + i + "reinforce");
				int reinforce = PlayerPrefs.GetInt("itemDataBase" + index + "reinforce", -1);
				if (reinforce >= 0)
				{
					PlayerPrefs.SetInt("itemDataBase" + index + "reinforce", reinforce + 1);
					return true;
				}
			}
		}
		return false;
	}

	public Item GetEquippedItem(int index)
	{
		if (equippedItemArray[index] < 0)
			return null;
		else
			return itemDataBase[equippedItemArray[index]];
	}

	public void SwapSlot(int index1, int index2)
	{
		int itemIndex1 = InventoryItemArray[index1];
		int itemIndex2 = InventoryItemArray[index2];
		SetSlot(index1, itemIndex2);
		SetSlot(index2, itemIndex1);
	}

	public void EquipItem(int equippedIndex, int originalIndex)
	{
		int equippedItemIndex = equippedItemArray[equippedIndex];
		int originalItemIndex = InventoryItemArray[originalIndex];
		SetEquipSlot(equippedIndex, originalItemIndex);
		SetSlot(originalIndex, equippedItemIndex);

		RefreshEquipStatus();
	}

	public void RefreshEquipStatus()
	{
		itemStatus.attackDamage = 0;
		itemStatus.armor = 0;
		itemStatus.hp = 0;
		itemStatus.hpRecovery = 0;
		itemStatus.moveSpeed = 0;

		for (int i = 0; i < equippedItemArray.Length; i++)
		{
			if (equippedItemArray[i] >= 0)
			{
				itemStatus += itemDataBase[equippedItemArray[i]].status;
			}
		}
	}

	void SetSlot(int index, int newItemIndex)
	{
		InventoryItemArray[index] = newItemIndex;
		PlayerPrefs.SetInt("inventory" + index, newItemIndex);
	}

	void SetEquipSlot(int index, int newItemIndex)
	{
		equippedItemArray[index] = newItemIndex;
		PlayerPrefs.SetInt("equip" + index, newItemIndex);
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

	public Item GetItem(string itemCode)
	{
		return itemTable.GetItem(itemCode);
	}

	public void SetPlayerSkill(int index, string skillCode)
	{
		PlayerPrefs.SetString("Skill" + index, skillCode);
		player.skillCodeArray[index] = skillCode;
	}

	public Skill GetSkill(string skillCode)
	{
		if (skillCode != "")
			return skillTable[skillCode];
		else
			return null;
	}

	public string GetPlayerSkillCode(int index)
	{
		return player.skillCodeArray[index];
	}

	public Skill[] GetSkillArray()
	{
		return skillTableElements;
	}
}