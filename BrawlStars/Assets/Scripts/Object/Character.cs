using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    public Status status;
    Status statusWithItem;
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < skillArray.Length; i++)
        {
            skillArray[i].MakeTargetRangeMesh();
            lastSkillActionTime[i] = Time.time - skillArray[i].cooldown;
        }

        EquipItem();
    }

    protected override void Update()
    {
        base.Update();
    }

    public Skill GetSkill(int index)
    {
        if (index >= skillArray.Length)
            return null;
        else
            return skillArray[index];
    }

    public void EquipItem()
    {
        statusWithItem = status;
        statusWithItem.attackDamage += GameManager.GetInstance().itemStatus.attackDamage;
        statusWithItem.armor += GameManager.GetInstance().itemStatus.armor;
        statusWithItem.hp += GameManager.GetInstance().itemStatus.hp;
        statusWithItem.hpRecovery += GameManager.GetInstance().itemStatus.hpRecovery;
        statusWithItem.moveSpeed += GameManager.GetInstance().itemStatus.moveSpeed;
    }
}