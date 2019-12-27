using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int upIndex = 3;
	public static int upFrontIndex = 2;
	public static int frontIndex = 1;
    public static int downFrontIndex = 0;
	public static int downIndex = 4;

	public static float ConvertIn2PI(float value, float min)
    {
        float result = value;
        while (result < min)
            result += Mathf.PI * 2;
        while (result > min + Mathf.PI * 2)
            result -= Mathf.PI * 2;
        return result;
    }
}

[System.Serializable]
public struct Status
{
    public int attackDamage;
    public int armor;
    public int hp;
    public float hpRecovery;
    public float moveSpeed;

    public static Status operator+(Status a, Status b)
    {
        Status result;
        result.attackDamage = a.attackDamage + b.attackDamage;
        result.armor = a.armor + b.armor;
        result.hp = a.hp + b.hp;
        result.hpRecovery = a.hpRecovery + b.hpRecovery;
        result.moveSpeed = a.moveSpeed + b.moveSpeed;
        return result;
	}

	public static Status operator *(Status a, int b)
	{
		Status result;
		result.attackDamage = a.attackDamage * b;
		result.armor = a.armor * b;
		result.hp = a.hp * b;
		result.hpRecovery = a.hpRecovery * b;
		result.moveSpeed = a.moveSpeed * b;
		return result;
	}
}

[System.Serializable]
public struct Menu
{
	public string name;
	public GameObject content;
}

[System.Serializable]
public struct DropItem
{
	public float percentage;
	public string itemCode;
}

[System.Serializable]
public struct DropItemList
{
	public string monsterName;
	public DropItem[] itemList;
}

[System.Serializable]
public struct SpriteIndex
{
    public int start;
    public int end;
}

[System.Serializable]
public enum Team
{
    Player, Enemy
}

public enum State
{
    Idle, Attack, Dead
}

public enum ItemType
{
	WEAPON, ARMOR, ACCESSORY, SUB, ETC
}

[System.Serializable]
public enum SkillType
{
	Passive, Active, Ultimate, Monster
}

[System.Serializable]
public struct SkillListElement
{
	public SkillType type;
	public GameObject scrollRectContent;
	public int skillCountPerLine;
	public int skillIndex;
}

public enum SlotType
{
	Normal, Equip
}