using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public SkillJoystick joystick;
    public int skillIndex;
    public Text cooldownText;
    public Character player;

    bool onSet = false;

    Image iconImage;

    void Start()
    {
        player = BattleManager.GetInstance().player;
    }

    void Update()
    {
        if (!onSet)
        {
            if (skillIndex < player.skillArray.Length)
            {
                iconImage = GetComponent<Image>();
                iconImage.sprite = player.skillArray[skillIndex].icon;
            }
            else
            {
                Destroy(gameObject);
            }

            onSet = !onSet;
        }
        
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (skillIndex < player.skillArray.Length && ReadyToAction())
        {
            joystick.gameObject.SetActive(true);
            joystick.skillIndex = skillIndex;
            joystick.OnPointerDown(eventData);

            Color color = GetComponent<Image>().color;
            color.a = 0f;
            GetComponent<Image>().color = color;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystick.gameObject.activeSelf == true)
        {
            joystick.OnDrag(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (joystick.gameObject.activeSelf == true)
        {
            joystick.OnPointerUp(eventData);

            joystick.gameObject.SetActive(false);
            Color color = GetComponent<Image>().color;
            color.a = 1f;
            GetComponent<Image>().color = color;
        }
    }

    public bool ReadyToAction()
    {
        return player.GetRemainSkillCooldown(skillIndex) < 0;
    }
}
