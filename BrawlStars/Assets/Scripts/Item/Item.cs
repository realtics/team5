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
		if (reinforce > 1)
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

		Status statusWithReinforce = GetStatusWithReinforce();
        if (status.attackDamage > 0)
            result += "공격력 : " + statusWithReinforce.attackDamage + "\n";
        if (status.armor > 0)
            result += "방어력 : " + statusWithReinforce.armor + "\n";
        if (status.hp > 0)
            result += "체력 : " + statusWithReinforce.hp + "\n";
        if (status.hpRecovery > 0)
            result += "체력회복 : " + statusWithReinforce.hpRecovery + "\n";
        if (status.moveSpeed > 0)
            result += "이동속도 : " + statusWithReinforce.moveSpeed + "\n";

        result += "\n" + etc;

        return result;
    }

	public Item(Item other, int value = 1)
	{
		count = value;
		reinforce = value;
		itemCode = other.itemCode;
		itemName = other.itemName;
		icon = other.icon;
		type = other.type;
		status = other.status;
		statusPerReinforce = other.statusPerReinforce;
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

	public void Reinforce(Item material)
	{
		reinforce += material.reinforce;
	}

	public int GetReinforceValue()
	{
		return reinforce;
	}

	public Status GetStatusWithReinforce()
	{
		return status + statusPerReinforce * (reinforce - 1);
	}
}
