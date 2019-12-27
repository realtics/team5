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
	public float cooldown;
	public int hpRecovery;

	int itemIndex;
	int value;

	public string GetItemExplanation()
    {
        string result = itemName + "\n\n";
		if (type == ItemType.ETC)
			etc = etc.Replace("[[HP]]", hpRecovery.ToString());
		if (value > 1 && type != ItemType.ETC)
			result = "+" + value + " " + result;

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

	public string ValueToString()
	{
		if (value > 1)
			return value.ToString();
		else
			return "";
	}

	public bool IsDeleted()
	{
		return value <= 0;
	}

	public Item(Item other, int index, int _value = 1)
	{
		itemIndex = index;
		value = _value;
		itemCode = other.itemCode;
		itemName = other.itemName;
		icon = other.icon;
		type = other.type;
		status = other.status;
		statusPerReinforce = other.statusPerReinforce;
		etc = other.etc;
		cooldown = other.cooldown;
		hpRecovery = other.hpRecovery;

		PlayerPrefs.SetString("itemDataBase" + itemIndex + "name", itemCode);
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
	}

	public void AddOneCount()
	{
		value++;
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
	}

	public void Reinforce(Item material)
	{
		value += material.value;
		PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
	}

	public int GetReinforceValue()
	{
		return value;
	}

	public Status GetStatusWithReinforce()
	{
		return status + statusPerReinforce * (value - 1);
	}

	public virtual void Activate(Character player)
	{
		value--;
		player.TakeDamage(-hpRecovery);
		if (value > 0)
			PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
		else
			GameManager.GetInstance().RemoveItem(itemIndex);
	}

	public void Delete()
	{
		PlayerPrefs.DeleteKey("itemDataBase" + itemIndex + "name");
		PlayerPrefs.DeleteKey("itemDataBase" + itemIndex + "value");
	}
}
