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
	int[] InventorySlotArray;
	int[] equippedSlotArray;
	Status itemStatus;
	public DropTable dropTable = null;

	Dictionary<string, Skill> skillTable;
	public Skill[] skillTableElements;

	[HideInInspector]
	public int stageIndex;
	public Action RefreshSlots;

	public ReinforceMaterial[] reinforceMaterial;
	Dictionary<ItemType, string> reinforceTable;

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
		int dataBaseLength = PlayerPrefs.GetInt("itemDataBaseLength", 0);
		int j = 0;
		for (int i = 0; j < dataBaseLength; i++)
		{
			string itemCode = PlayerPrefs.GetString("itemDataBase" + i + "name", "");
			if (itemCode != "")
			{
				Item item = new Item(itemTable.GetItem(itemCode), i, PlayerPrefs.GetInt("itemDataBase" + i + "value", 1));
				itemDataBase.Add(i, item);
				j++;
			}
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

		reinforceTable = new Dictionary<ItemType, string>();
		for(int i = 0; i < reinforceMaterial.Length; i++)
		{
			reinforceTable.Add(reinforceMaterial[i].type, reinforceMaterial[i].materialCode);
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
		InventorySlotArray = new int[row * column];
		for (int i = 0; i < InventorySlotArray.Length; i++)
		{
			InventorySlotArray[i] = PlayerPrefs.GetInt("inventory" + i, -1);
			if (InventorySlotArray[i] > 0 && !itemDataBase.ContainsKey(InventorySlotArray[i]))
				PlayerPrefs.DeleteKey("inventory" + i);
		}
	}

	public void InitEquipSlot(int count)
	{
		equippedSlotArray = new int[count];
		for (int i = 0; i < count; i++)
		{
			equippedSlotArray[i] = PlayerPrefs.GetInt("equip" + i, -1);
			if (equippedSlotArray[i] > 0 && !itemDataBase.ContainsKey(equippedSlotArray[i]))
				PlayerPrefs.DeleteKey("equip" + i);
		}
		RefreshEquipStatus();
	}

	public void ClearInventory()
	{
		for (int i = 0; i < InventorySlotArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("inventory" + i);
			InventorySlotArray[i] = -1;
		}
		for (int i = 0; i < equippedSlotArray.Length; i++)
		{
			PlayerPrefs.DeleteKey("equip" + i);
			equippedSlotArray[i] = -1;
		}
		foreach (int i in itemDataBase.Keys)
		{
			PlayerPrefs.DeleteKey("itemDataBase" + i + "name");
			PlayerPrefs.DeleteKey("itemDataBase" + i + "value");
		}
		itemDataBase.Clear();
		RefreshSlots();
		PlayerPrefs.SetInt("itemDataBaseLength", 0);
	}

	public bool AddNewItemInInventory(string itemCode)
	{
		int possibleSlotIndex = FindMinimumPossibleSlotIndex(itemCode);

		if (possibleSlotIndex >= 0)
		{
			if (InventorySlotArray[possibleSlotIndex] < 0)
			{
				AddNewItem(itemCode, possibleSlotIndex);
			}
			else
			{
				Item newItem = itemDataBase[InventorySlotArray[possibleSlotIndex]];
				newItem.AddOneCount();
			}

			return true;
		}

		return false;
	}

	public void AddNewItem(string itemCode, int slotIndex)
	{
		int itemIndex = 0;
		while(true)
		{
			string itemName = PlayerPrefs.GetString("itemDataBase" + itemIndex + "name", "");
			if (itemName == "")
				break;
			itemIndex++;
		}
		Item newItem = new Item(itemTable.GetItem(itemCode), itemIndex);
		SetSlot(slotIndex, itemIndex);
		itemDataBase.Add(itemIndex, newItem);
		PlayerPrefs.SetInt("itemDataBaseLength", itemDataBase.Count);
	}

	public void RemoveItem(int key)
	{
		if (itemDataBase.ContainsKey(key))
		{
			itemDataBase[key].Delete();
			itemDataBase.Remove(key);
			PlayerPrefs.SetInt("itemDataBaseLength", itemDataBase.Count);

			for (int i = 0; i < InventorySlotArray.Length; i++)
			{
				if (InventorySlotArray[i] == key)
				{
					InventorySlotArray[i] = -1;
					PlayerPrefs.DeleteKey("inventory" + i);
				}
			}

			for (int i = 0; i < equippedSlotArray.Length; i++)
			{
				if (equippedSlotArray[i] == key)
				{
					equippedSlotArray[i] = -1;
					PlayerPrefs.DeleteKey("equip" + i);
				}
			}
		}
	}

	int FindMinimumPossibleSlotIndex(string itemCode)
	{
		int sameItemSlotIndex;
		int emptySlotIndex = -1;
		for (sameItemSlotIndex = 0; sameItemSlotIndex < InventorySlotArray.Length; sameItemSlotIndex++)
		{
			if (InventorySlotArray[sameItemSlotIndex] < 0)
			{
				if (emptySlotIndex < 0)
					emptySlotIndex = sameItemSlotIndex;
			}
			else
			{
				Item itemInSlot = itemDataBase[InventorySlotArray[sameItemSlotIndex]];
				if ((itemInSlot.type == ItemType.ETC || itemInSlot.type == ItemType.POTION) && itemInSlot.itemCode == itemCode)
					return sameItemSlotIndex;
			}
		}
		return emptySlotIndex;
	}

	public Item GetItemInInventory(int index)
	{
		if (itemDataBase.ContainsKey(InventorySlotArray[index]))
			return itemDataBase[InventorySlotArray[index]];
		else
			return null;
	}

	public bool Reinforce(string itemCode, int index)
	{
		for (int materialIndex = 0; materialIndex < InventorySlotArray.Length; materialIndex++)
		{
			int materialKey = InventorySlotArray[materialIndex];
			int originalKey = InventorySlotArray[index];

			bool equipable = itemDataBase[originalKey].type != ItemType.ETC;
			bool isEqualItem = itemDataBase.ContainsKey(materialKey) && itemDataBase[materialKey].itemCode == itemCode;
			bool isMaterial = itemDataBase.ContainsKey(materialKey) && itemDataBase[materialKey].itemCode == reinforceTable[itemDataBase[originalKey].type];
			if (equipable && materialIndex != index && (isEqualItem || isMaterial))
			{

				if (isEqualItem)
				{
					for (int i = 0; i < itemDataBase[materialKey].GetReinforceValue(); i++)
						itemDataBase[originalKey].AddOneCount();
					RemoveItem(materialKey);
				}
				else if (isMaterial)
				{
					itemDataBase[originalKey].AddOneCount();
					itemDataBase[materialKey].SubtractOneCount();
				}

				RefreshSlots();
				return true;
			}
		}
		return false;
	}

	public bool Break(int index)
	{
		int key = InventorySlotArray[index];
		bool result = AddNewItemInInventory(reinforceTable[itemDataBase[key].type]);
		if(result)
		{
			itemDataBase[key].SubtractOneCount();
			RefreshSlots();
			return true;
		}
		return false;
	}

	public Item GetEquippedItem(int index)
	{
		if (itemDataBase.ContainsKey(equippedSlotArray[index]))
			return itemDataBase[equippedSlotArray[index]];
		else
			return null;
	}

	public void SwapSlot(int index1, int index2)
	{
		int itemIndex1 = InventorySlotArray[index1];
		int itemIndex2 = InventorySlotArray[index2];
		SetSlot(index1, itemIndex2);
		SetSlot(index2, itemIndex1);
	}

	public void EquipItem(int equippedIndex, int originalIndex)
	{
		int equippedItemIndex = equippedSlotArray[equippedIndex];
		int originalItemIndex = InventorySlotArray[originalIndex];
		SetEquipSlot(equippedIndex, originalItemIndex);
		SetSlot(originalIndex, equippedItemIndex);

		RefreshEquipStatus();
	}

	public Item GetQuickSlotItem()
	{
		int key = equippedSlotArray[equippedSlotArray.Length - 1];
		if (itemDataBase.ContainsKey(key))
			return itemDataBase[key];
		else
			return null;
	}

	public void RefreshEquipStatus()
	{
		itemStatus.attackDamage = 0;
		itemStatus.armor = 0;
		itemStatus.hp = 0;
		itemStatus.hpRecovery = 0;
		itemStatus.moveSpeed = 0;

		for (int i = 0; i < equippedSlotArray.Length - 1; i++)
		{
			if (itemDataBase.ContainsKey(equippedSlotArray[i]))
			{
				Item equippedItem = itemDataBase[equippedSlotArray[i]];
				itemStatus += equippedItem.GetStatusWithReinforce();
			}
		}
	}

	void SetSlot(int index, int newItemIndex)
	{
		InventorySlotArray[index] = newItemIndex;
		if (newItemIndex < 0)
			PlayerPrefs.DeleteKey("inventory" + index);
		else
			PlayerPrefs.SetInt("inventory" + index, newItemIndex);
	}

	void SetEquipSlot(int index, int newItemIndex)
	{
		equippedSlotArray[index] = newItemIndex;
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