using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new DropTable", menuName = "Scriptable Object/Drop Table", order = int.MaxValue)]
public class DropTable : ScriptableObject
{
	public DropItemList[] dropList;
	Dictionary<string, DropItem[]> dropTable;
	
	public void Init()
	{
		dropTable = new Dictionary<string, DropItem[]>();
		for (int i = 0; i < dropList.Length; i++)
		{
			dropTable.Add(dropList[i].monsterName, dropList[i].itemList);
		}
	}

	public DropItem[] GetDropItemList(string monsterName)
	{
		return dropTable[monsterName];
	}
}
