using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{    
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < skillArray.Length; i++)
        {
            skillArray[i].MakeTargetRangeMesh();
            lastSkillActionTime[i] = Time.time - skillArray[i].cooldown;
        }

        finalStatus = GameManager.GetInstance().GetFinalStatus();
        hpBar.SetMaxHp(finalStatus.hp);
        hpBar.SetHp(finalStatus.hp);
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
<<<<<<< HEAD

    public void Revival()
    {
        this.gameObject.SetActive(true);

        hpBar.gameObject.SetActive(true);
        this.currentHp = this.maxHp;
        hpBar.SetMaxHp(this.maxHp);
        hpBar.SetHp(this.currentHp);
        
        this.state = State.Idle;

        spriteDirectionCount = 5;
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Debug.Log(collider.gameObject.name);
            Destroy(collider.gameObject);
        }
    }
=======
>>>>>>> master
}