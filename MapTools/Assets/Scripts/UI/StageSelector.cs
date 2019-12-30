using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelector : MonoBehaviour, IPointerClickHandler
{
	public int index;
	RectTransform rectTransform;
	Vector2 originalSize;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		originalSize = rectTransform.sizeDelta;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Select();
	}

	public void Select()
	{
		RoomManager.GetInstance().SelectStage(index);
		RoomManager.GetInstance().SizeDownAllStageSelector();
		SizeUp();
	}

	public void SizeUp()
	{
		rectTransform.sizeDelta = originalSize;
	}

	public void SizeDown()
	{
		rectTransform.sizeDelta = originalSize * 0.8f;
	}
}
