using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemCode;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public Status status;
	public Status statusPerReinforce;
    public string etc;

	int count;
	int reinforce;

	public string GetItemExplanation()
    {
        string result = itemName + "\n\n";
		if (reinforce > 0)
			result = "+" + reinforce + " " + result;

		switch (type)
        {
            case ItemType.WEAPON:
                result += "무기\n";
                break;
            case ItemType.ARMOR:
                result += "방어구\n";
                break;
            case ItemType.ACCESSORY:
                result += "악세사리\n";
                break;
            case ItemType.SUB:
                result += "보조장비\n";
                break;
            default:
                break;
        }

        if (status.attackDamage > 0)
            result += "공격력 : " + status.attackDamage + "\n";
        if (status.armor > 0)
            result += "방어력 : " + status.armor + "\n";
        if (status.hp > 0)
            result += "체력 : " + status.hp + "\n";
        if (status.hpRecovery > 0)
            result += "체력회복 : " + status.hpRecovery + "\n";
        if (status.moveSpeed > 0)
            result += "이동속도 : " + status.moveSpeed + "\n";

        result += "\n" + etc;

        return result;
    }

	public Item(Item other)
	{
		count = 1;
		reinforce = 0;
		itemCode = other.itemCode;
		itemName = other.itemName;
		icon = other.icon;
		type = other.type;
		status = other.status;
		etc = other.etc;
	}

	public void AddOneCount()
	{
		count++;
	}

	public int GetCount()
	{
		return count;
	}
}
