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
		int databaseIndex = 0;
		int length = PlayerPrefs.GetInt("itemDataBaseLength", 0);
		for (int i = 0; databaseIndex < length; i++)
		{
			string itemCode = PlayerPrefs.GetString("itemDataBase" + databaseIndex + "name", "");
			Item item = new Item(itemTable.GetItem(itemCode), PlayerPrefs.GetInt("itemDataBase" + i + "reinforce", 1));
			itemDataBase.Add(databaseIndex, item);
			databaseIndex++;
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
				AddNewItem(newItem, possibleSlotIndex);
			}
			else
			{
				Item newItem = itemDataBase[InventoryItemArray[possibleSlotIndex]];
				newItem.AddOneCount();
				PlayerPrefs.SetInt("itemDataBase" + InventoryItemArray[possibleSlotIndex] + "count", newItem.GetCount());
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void AddNewItem(Item newItem, int slotIndex)
	{
		int itemIndex = 0;
		while(true)
		{
			string itemName = PlayerPrefs.GetString("itemDataBase" + itemIndex + "name", "");
			if (itemName == "")
				break;
			itemIndex++;
		}
		PlayerPrefs.SetString("itemDataBase" + itemIndex + "name", newItem.itemCode);
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "count", 1);
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "reinforce", 1);
		SetSlot(slotIndex, itemIndex);
		itemDataBase.Add(itemIndex, newItem);
	}

	public void RemoveItem(int key)
	{
		if (itemDataBase.ContainsKey(key))
		{
			itemDataBase.Remove(key);
			PlayerPrefs.DeleteKey("itemDataBase" + key + "name");
			PlayerPrefs.DeleteKey("itemDataBase" + key + "count");
			PlayerPrefs.DeleteKey("itemDataBase" + key + "reinforce");
		}
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

	public bool ReinforceSuccess(string itemCode, int index, out int materialIndex)
	{
		for (materialIndex = 0; materialIndex < InventoryItemArray.Length; materialIndex++)
		{
			int materialKey = InventoryItemArray[materialIndex];
			int originalKey = InventoryItemArray[index];

			bool equipable = itemDataBase[originalKey].type != ItemType.ETC;
			bool isHaveMaterial = itemDataBase.ContainsKey(materialKey) && itemDataBase[materialKey].itemCode == itemCode;
			if (equipable && materialIndex != index && isHaveMaterial)
			{
				itemDataBase[originalKey].Reinforce(itemDataBase[materialKey]);
				PlayerPrefs.SetInt("itemDataBase" + originalKey + "reinforce", itemDataBase[originalKey].GetReinforceValue());

				InventoryItemArray[materialIndex] = -1;
				SetSlot(materialIndex, -1);
				RemoveItem(materialKey);

				RefreshSlots();
				return true;
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
				Item equippedItem = itemDataBase[equippedItemArray[i]];
				itemStatus += equippedItem.GetStatusWithReinforce();
			}
		}
	}

	void SetSlot(int index, int newItemIndex)
	{
		InventoryItemArray[index] = newItemIndex;
		if (newItemIndex < 0)
			PlayerPrefs.DeleteKey("inventory" + index);
		else
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