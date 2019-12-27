using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : ControlUI
{
	float lastUseTime;
	Character player;
	Item item;

	public Image icon;
	public Text countText;
	public Text cooldownText;

	public void Init(Character _player, Item _item)
	{
		player = _player;
		item = _item;

		Color color = icon.color;
		if (item != null)
		{
			color.a = 1;
			icon.sprite = item.icon;
			countText.text = item.ValueToString();
			lastUseTime = Time.time - item.cooldown;
		}
		else
		{
			color.a = 0;
		}
		icon.color = color;
	}

	void Update()
	{
		if (item != null)
		{
			PrintRemainCooldown();
		}
	}

	void PrintRemainCooldown()
	{
		if (Time.time - lastUseTime > item.cooldown)
		{
			icon.color = new Color(1, 1, 1, icon.color.a);
			cooldownText.text = "";
		}
		else
		{
			icon.color = new Color(0.3f, 0.3f, 0.3f, icon.color.a);
			cooldownText.text = Mathf.Ceil(item.cooldown - (Time.time - lastUseTime)).ToString();
		}
	}

	public override void Cancel()
	{

	}

	public override void Drag(Vector2 position)
	{

	}

	public override void PointerDown(Vector2 position)
	{

	}

	public override void PointerUp(Vector2 position)
	{
		if (item != null && Time.time - lastUseTime > item.cooldown)
		{
			item.Activate(player);
			lastUseTime = Time.time;
			if (item.IsDeleted())
				item = null;
			else
				countText.text = item.ValueToString();
		}
	}
}
