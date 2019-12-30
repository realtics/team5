using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScroll : MonoBehaviour, IPointerDownHandler
{
	public ButtonScrollRect buttonScroll;

	public bool isDownButton;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (isDownButton)
		{
			buttonScroll.ButtonDownisPressed();
		}
		else 
		{
			buttonScroll.ButtonUpisPressed();
		}
	}
}
