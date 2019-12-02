using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ControlUI : MonoBehaviour
{
	public abstract void PointerDown(PointerEventData eventData);
	public abstract void Drag(PointerEventData eventData);
	public abstract void PointerUp(PointerEventData eventData);
}
