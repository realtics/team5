using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ControlUI : MonoBehaviour
{
	public abstract void PointerDown(Vector2 position);
	public abstract void Drag(Vector2 position);
	public abstract void PointerUp(Vector2 position);
	public abstract void Cancel();
}
