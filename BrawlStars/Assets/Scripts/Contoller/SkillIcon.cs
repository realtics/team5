using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIcon : ControlUI
{
    public SkillJoystick joystick;
    public int skillIndex;
    Skill skill;
    public Text cooldownText;
    public Character player;

    bool onSkillActivate;

    Image iconImage;
    MeshFilter rangeObject;

    void Start()
    {
		onSkillActivate = false;
    }

	public void Init(Character player)
	{
		this.player = player;

		if (skillIndex < player.skillCodeArray.Length)
		{
			skill = GameManager.GetInstance().GetSkill(player.skillCodeArray[skillIndex]);
			if (skill != null)
			{
				iconImage = GetComponent<Image>();
				iconImage.sprite = skill.icon;
			}
		}

		if(iconImage == null)
			Destroy(gameObject);
	}

    void Update()
    {        
        if (skillIndex < player.skillCodeArray.Length)
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
            rangeObject = ObjectPool.GetInstance().GetObject(BattleManager.GetInstance().rangeObject.gameObject).GetComponent<MeshFilter>();
            rangeObject.mesh = skill.GetTargetRangeMesh();
            rangeObject.transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);

            if (!skill.isActivatedInPlayerPosition)
            {
                joystick.gameObject.SetActive(true);
                joystick.Init(rangeObject, skillIndex);
                joystick.PointerDown(position);

                Color color = iconImage.color;
                color.a = 0f;
                iconImage.color = color;
            }

			onSkillActivate = true;
		}
    }

    public override void Drag(Vector2 position)
    {
		if (joystick.gameObject.activeSelf)
		{
			joystick.Drag(position);
		}
		else
		{
			rangeObject.transform.position = player.transform.position;
		}
    }

	public override void PointerUp(Vector2 position)
    {
        if (onSkillActivate)
        {
            player.AttackProcess(skillIndex, rangeObject.transform.position, rangeObject.transform.rotation.eulerAngles.y);
            ObjectPool.GetInstance().PushObject(rangeObject.gameObject);
            if (joystick.gameObject.activeSelf)
                joystick.PointerUp(position);
            Cancel();
		}
    }

	public override void Cancel()
    {
        if (joystick.gameObject.activeSelf)
        {
            joystick.Cancel();
            joystick.gameObject.SetActive(false);
        }

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
