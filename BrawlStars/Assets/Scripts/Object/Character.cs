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

    public void Revival()
    {
        this.gameObject.SetActive(true);

        hpBar.gameObject.SetActive(true);
        currentHp = status.hp;
        hpBar.SetMaxHp(status.hp);
        hpBar.SetHp(currentHp);

        this.state = State.Idle;

        spriteDirectionCount = 5;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Debug.Log(collider.gameObject.name);
            Destroy(collider.gameObject);
        }
    }
}