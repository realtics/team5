﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int upIndex = 3;
    public static int downIndex = 4;
    public static int frontIndex = 1;
    public static int downFrontIndex = 0;
    public static int upFrontIndex = 2;

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
}