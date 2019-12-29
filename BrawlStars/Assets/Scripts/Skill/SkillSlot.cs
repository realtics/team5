using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	string skillCode;
	Skill skill;
	Image image;
	SkillSetter setter;

	public SkillType type;
	public int skillSlotIndex = -1;

	bool isDragged;

	Image skillWindow;
	Text skillNameText;
	Text skillTooltipText;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	public void Init(SkillSetter _setter, int index, string _skillCode, Image _skillWindow, Text _skillNameText, Text _skillTooltipText)
	{
		setter = _setter;
		skillSlotIndex = index;
		SetSkill(_skillCode);
		skillWindow = _skillWindow;
		skillNameText = _skillNameText;
		skillTooltipText = _skillTooltipText;
	}

	void SetSkill(string _skillCode)
	{
		skillCode = _skillCode;
		skill = GameManager.GetInstance().GetSkill(skillCode);
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
		isDragged = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		isDragged = true;
		RoomManager.GetInstance().DragSkillImage(image.sprite, eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (isDragged)
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
		} else
		{
			skillWindow.gameObject.SetActive(true);
			skillNameText.text = skill.skillName;
			skillTooltipText.text = skill.GetTooltip();
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
