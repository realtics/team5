using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetter : MonoBehaviour
{
	public SkillSlot slotPrefab;
	public SkillListElement[] skillList;
	[HideInInspector]
	public SkillSlot dragSkillTargetSlot;

	private void Awake()
	{
		dragSkillTargetSlot = null;
		for (int i = 0; i < skillList.Length; i++)
		{
			skillList[i].skillIndex = 0;
		}
	}

	void Start()
	{
		Skill[] skillArray = GameManager.GetInstance().GetSkillArray();
		for (int i = 0; i < skillArray.Length; i++)
		{
			Skill skill = GameManager.GetInstance().GetSkill(skillArray[i].skillCode);
			SkillSlot newSlot = Instantiate(slotPrefab);
			newSlot.Init(this, skillArray[i].skillCode);

			for (int j = 0; j < skillList.Length; j++)
			{
				if (skill.type == skillList[j].type)
				{
					newSlot.transform.SetParent(skillList[j].scrollRectContent.transform);
					skillList[j].skillIndex++;
				}
			}

			newSlot.transform.position = new Vector3(0, 0, 0);
			newSlot.transform.localScale = new Vector3(1, 1, 1);
		}
	}
}
