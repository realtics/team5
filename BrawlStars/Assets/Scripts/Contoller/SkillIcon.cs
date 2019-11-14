﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIcon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public SkillJoystick joystick;
    public Skill skill;
    public Text cooldownText;

    Image iconImage;

    void Start()
    {
        iconImage = GetComponent<Image>();
        skill.InitCooldown();
    }

    void Update()
    {
        if(skill.ReadyToAction())
        {
            iconImage.color = new Color(1, 1, 1, iconImage.color.a);
            cooldownText.text = "";
        }
        else
        {
            iconImage.color = new Color(0.3f, 0.3f, 0.3f, iconImage.color.a);
            cooldownText.text = Mathf.Ceil(skill.GetRemainCooldown()).ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (skill.ReadyToAction())
        {
            joystick.gameObject.SetActive(true);
            joystick.skill = skill;
            joystick.OnPointerDown(eventData);

            Color color = GetComponent<Image>().color;
            color.a = 0f;
            GetComponent<Image>().color = color;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill.ReadyToAction())
        {
            joystick.OnDrag(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (skill.ReadyToAction())
        {
            joystick.OnPointerUp(eventData);

            joystick.gameObject.SetActive(false);
            Color color = GetComponent<Image>().color;
            color.a = 1f;
            GetComponent<Image>().color = color;
        }
    }
}