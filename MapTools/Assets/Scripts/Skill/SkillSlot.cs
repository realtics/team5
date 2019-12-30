using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	string skillCode;
	Image image;
	public SkillSetter setter;

	public SkillType type;
	public int skillSlotIndex = -1;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void Start()
    {
		if(skillSlotIndex >= 0)
		{
			SetSkill(GameManager.GetInstance().GetPlayerSkillCode(skillSlotIndex));
		}
    }

	public void Init(SkillSetter _setter, string _skillCode)
	{
		setter = _setter;
		skillSlotIndex = -1;
		SetSkill(_skillCode);
	}

	void SetSkill(string _skillCode)
	{
		skillCode = _skillCode;
		Skill skill = GameManager.GetInstance().GetSkill(skillCode);
		Color color = image.color;
		if (skill != null)
		{
			color.a = 1.0f;
			image.sprite = skill.icon;
		}
		else
		{
			color.a = 0.0f;
			image.sprite = null;
		}
		image.color = color;
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnDrag(PointerEventData eventData)
	{
		RoomManager.GetInstance().DragSkillImage(image.sprite, eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		RoomManager.GetInstance().DragFinish();
		SkillSlot targetSlot = setter.dragSkillTargetSlot;
		Skill skill = GameManager.GetInstance().GetSkill(skillCode);
		if (targetSlot != null && targetSlot.skillSlotIndex >= 0)
		{
			if (targetSlot.type == skill.type)
			{
				setter.dragSkillTargetSlot.SetSkill(skillCode);
				GameManager.GetInstance().SetPlayerSkill(setter.dragSkillTargetSlot.skillSlotIndex, skillCode);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		setter.dragSkillTargetSlot = this;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		setter.dragSkillTargetSlot = null;
	}
}
