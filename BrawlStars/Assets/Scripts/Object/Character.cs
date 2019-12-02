using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
	public InGameLog inGameLog;

	protected override void Awake()
	{
		base.Awake();

		for (int i = 0; i < skillArray.Length; i++)
		{
			skillArray[i].MakeTargetRangeMesh();
			lastSkillActionTime[i] = Time.time - skillArray[i].cooldown;
		}
	}

	protected override void Start()
    {
        base.Start();

		finalStatus = GameManager.GetInstance().GetFinalStatus();
        hpBar.SetMaxHp(finalStatus.hp);
        hpBar.SetHp(finalStatus.hp);
		inGameLog = GetComponent<InGameLog>();
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
				inGameLog.GameLog(collider.gameObject.name);
				Debug.Log(collider.gameObject.name);
                Destroy(collider.gameObject);
            }
        }
    }
}