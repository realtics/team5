using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string itemCode;
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public Status status;
    public Status statusPerReinforce;
    public string tooltip;
    public float cooldown;

    public int hpRecovery;

    protected int itemIndex;
    protected int value;

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
        tooltip = other.tooltip;
        cooldown = other.cooldown;
        hpRecovery = other.hpRecovery;

        PlayerPrefs.SetString("itemDataBase" + itemIndex + "name", itemCode);
        PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
    }

    public virtual void GetItemExplanation(ref Text nameText, ref Text tooltipText)
    {
        if (value > 1 && type != ItemType.ETC)
            nameText.text = "+" + value + " " + itemName;
        else
            nameText.text = itemName;

        tooltipText.text = "";

        switch (type)
        {
            case ItemType.WEAPON:
                tooltipText.text = "무기\n";
                break;
            case ItemType.ARMOR:
                tooltipText.text = "방어구\n";
                break;
            case ItemType.ACCESSORY:
                tooltipText.text = "악세사리\n";
                break;
            case ItemType.SUB:
                tooltipText.text = "보조장비\n";
                break;
            case ItemType.POTION:
                tooltipText.text = tooltip.Replace("[[HP]]", hpRecovery.ToString()).Replace("\\n", "\n");
                break;
            default:
                break;
        }

        Status statusWithReinforce = GetStatusWithReinforce();
        if (status.attackDamage > 0)
            tooltipText.text += "공격력 : " + statusWithReinforce.attackDamage + "\n";
        if (status.armor > 0)
            tooltipText.text += "방어력 : " + statusWithReinforce.armor + "\n";
        if (status.hp > 0)
            tooltipText.text += "체력 : " + statusWithReinforce.hp + "\n";
        if (status.hpRecovery > 0)
            tooltipText.text += "체력회복 : " + statusWithReinforce.hpRecovery + "\n";
        if (status.moveSpeed > 0)
            tooltipText.text += "이동속도 : " + statusWithReinforce.moveSpeed + "\n";
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

    public void AddOneCount()
    {
        value++;
        PlayerPrefs.SetInt("itemDataBase" + itemIndex + "value", value);
    }

    public void SubtractOneCount()
    {
        value--;
        if (value <= 0)
            GameManager.GetInstance().RemoveItem(itemIndex);
        else
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

    public void Delete()
    {
        PlayerPrefs.DeleteKey("itemDataBase" + itemIndex + "name");
        PlayerPrefs.DeleteKey("itemDataBase" + itemIndex + "value");
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
}
