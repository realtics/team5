using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIcon : ControlUI
{
    public SkillJoystick joystick;
    public int skillIndex;
    public Text cooldownText;
    public Character player;

    bool onSkillActivate;

    Image iconImage;

    void Start()
    {
		onSkillActivate = false;
	}

	public void Init(Character player)
	{
		this.player = player;

		if (skillIndex < player.skillArray.Length)
		{
			iconImage = GetComponent<Image>();
			iconImage.sprite = player.skillArray[skillIndex].icon;
		}
		else
		{
			Destroy(gameObject);
		}
	}

    void Update()
    {        
        if (skillIndex < player.skillArray.Length)
        {
            PrintRemainCooldown();
        }
    }

    void PrintRemainCooldown()
    {
        if (ReadyToAction())
        {
            iconImage.color = new Color(1, 1, 1, iconImage.color.a);
            cooldownText.text = "";
        }
        else
        {
            iconImage.color = new Color(0.3f, 0.3f, 0.3f, iconImage.color.a);
            cooldownText.text = Mathf.Ceil(player.GetRemainSkillCooldown(skillIndex)).ToString();
        }
    }

    public override void PointerDown(PointerEventData eventData)
    {
        if (ReadyToAction() && joystick.gameObject.activeSelf == false)
        {
            joystick.gameObject.SetActive(true);
            joystick.skillIndex = skillIndex;
            joystick.PointerDown(eventData);

            Color color = GetComponent<Image>().color;
            color.a = 0f;
            GetComponent<Image>().color = color;

			onSkillActivate = true;
		}
    }

    public override void Drag(PointerEventData eventData)
    {
        if (onSkillActivate)
        {
            joystick.Drag(eventData);
        }
    }

	public override void PointerUp(PointerEventData eventData)
    {
        if (onSkillActivate)
        {
            joystick.PointerUp(eventData);

            joystick.gameObject.SetActive(false);
            Color color = GetComponent<Image>().color;
            color.a = 1f;
            GetComponent<Image>().color = color;

			onSkillActivate = false;
		}
    }

    public bool ReadyToAction()
    {
        return player.GetRemainSkillCooldown(skillIndex) < 0;
    }
}
