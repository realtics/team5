using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemTable", menuName = "Scriptable Object/Item Table", order = int.MaxValue)]
public class ItemTable : ScriptableObject
{
	public Item[] itemList;
	Dictionary<string, Item> itemTable;

	public void Init()
	{
		itemTable = new Dictionary<string, Item>();
		for (int i = 0; i < itemList.Length; i++)
		{
			itemTable.Add(itemList[i].itemCode, itemList[i]);
		}
	}

	public Item GetItem(string itemCode)
	{
		return itemTable[itemCode];
	}
}
