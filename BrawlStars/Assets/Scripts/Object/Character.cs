using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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

		for (int i = 0; i < skillCodeArray.Length; i++)
		{
			Skill skill = GameManager.GetInstance().GetSkill(skillCodeArray[i]);
			if(skill != null)
				lastSkillActionTime[i] = Time.time - skill.cooldown;
		}
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
            BattleManager.GetInstance().PickUpItem(collider.GetComponent<Item>());
        }
	}
}