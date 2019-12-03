using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
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

	public override void Alive()
	{
		base.Alive();

		finalStatus = GameManager.GetInstance().GetFinalStatus();
		hpBar.SetMaxHp(finalStatus.hp);
		hpBar.SetHp(finalStatus.hp);
		currentHp = finalStatus.hp;

		BattleManager.GetInstance().upperHPBar.SetMaxHp(finalStatus.hp);
		BattleManager.GetInstance().upperHPBar.SetHp(finalStatus.hp);
	}

	public override void SetHp(int _hp)
	{
		base.SetHp(_hp);
		BattleManager.GetInstance().upperHPBar.SetHp(currentHp);
	}

	void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Item contactItem = collider.GetComponent<Item>();
            bool canAddNewItem = GameManager.GetInstance().AddNewItemInInventory(contactItem);
            if (canAddNewItem)
            {
				Debug.Log(collider.gameObject.name);
                Destroy(collider.gameObject);
            }
        }
	}
}