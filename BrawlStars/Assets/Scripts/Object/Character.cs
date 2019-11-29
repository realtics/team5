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


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Item contactItem = collider.GetComponent<Item>();
            bool canAddNewItem = GameManager.GetInstance().AddNewItemInInventory(contactItem);
            if (canAddNewItem)
            {
                Debug.Log(collider.name);
                Destroy(collider.gameObject);
            }
        }
    }
}