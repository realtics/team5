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

    public override void PointerDown(Vector2 position)
    {
        if (ReadyToAction() && joystick.gameObject.activeSelf == false)
        {
            joystick.gameObject.SetActive(true);
            joystick.skillIndex = skillIndex;
            joystick.PointerDown(position);

            Color color = iconImage.color;
            color.a = 0f;
			iconImage.color = color;

			onSkillActivate = true;
		}
    }

    public override void Drag(Vector2 position)
    {
        if (joystick.gameObject.activeSelf)
        {
            joystick.Drag(position);
        }
    }

	public override void PointerUp(Vector2 position)
    {
        if (onSkillActivate)
        {
            joystick.PointerUp(position);
			Cancel();
		}
    }

	public override void Cancel()
	{
		joystick.Cancel();

		joystick.gameObject.SetActive(false);
		Color color = iconImage.color;
		color.a = 1f;
		iconImage.color = color;

		onSkillActivate = false;
	}

	public bool ReadyToAction()
    {
        return player.GetRemainSkillCooldown(skillIndex) < 0;
    }
}
