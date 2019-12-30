using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct ClickedObject
{
	public int index;
	public ControlUI targetObject;

	public ClickedObject(int id, ControlUI target)
	{
		index = id;
		targetObject = target;
	}
}

public class InputHandler : MonoBehaviour
{ 
	GraphicRaycaster raycast;
	public ControlUI[] controlUIobjects;

	PointerEventData[] eventDataArray;
	public int maxMultiTouchCount;
	List<RaycastResult> raycastResults;
	List<ClickedObject> clickedObjectList;

	Touch touchInfo;

	// Use this for initialization
	void Start()
	{
		raycast = GetComponent<GraphicRaycaster>();
#if UNITY_EDITOR
		eventDataArray = new PointerEventData[1];
#elif UNITY_ANDROID
		eventDataArray = new PointerEventData[maxMultiTouchCount];
#endif
		for (int i = 0; i < eventDataArray.Length; i++)
			eventDataArray[i] = new PointerEventData(null);
		raycastResults = new List<RaycastResult>();
		clickedObjectList = new List<ClickedObject>();
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
		eventDataArray[0].position = Input.mousePosition;

		HandlePointerDown();
		HandleDrag();
		HandlePointerUp();

#elif UNITY_ANDROID
		for (int i = 0; i < Input.touchCount && i < eventDataArray.Length; i++)
		{
			eventDataArray[i].position = Input.GetTouch(i).position;
		}

		HandleTouchDown();
		HandleTouchDrag();
		HandleTouchUp();
#endif
	}

	void HandlePointerDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			raycastResults.Clear();
			raycast.Raycast(eventDataArray[0], raycastResults);
			ClickObjects(eventDataArray[0].position, 0, raycastResults);
		}
	}

	void HandleDrag()
	{
		if (clickedObjectList.Count > 0)
			clickedObjectList[0].targetObject.Drag(Input.mousePosition);
	}

	void HandlePointerUp()
	{
		if (Input.GetMouseButtonUp(0) && clickedObjectList.Count > 0)
		{
			clickedObjectList[0].targetObject.PointerUp(Input.mousePosition);
			clickedObjectList.Clear();
		}
	}

	void HandleTouchDown()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began)
			{
				raycastResults.Clear();
				raycast.Raycast(eventDataArray[i], raycastResults);
				ClickObjects(eventDataArray[i].position, Input.GetTouch(i).fingerId, raycastResults);
			}
		}
	}

	void HandleTouchDrag()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			for (int j = 0; j < clickedObjectList.Count; j++)
			{
				if (Input.GetTouch(i).fingerId == clickedObjectList[j].index)
				{
					clickedObjectList[j].targetObject.Drag(Input.GetTouch(i).position);
				}
			}
		}
	}

	void HandleTouchUp()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			for(int j = 0; j < clickedObjectList.Count; j++)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch(i).fingerId == clickedObjectList[j].index)
				{
					clickedObjectList[j].targetObject.PointerUp(Input.GetTouch(i).position);
					clickedObjectList.RemoveAt(j);
				}
			}
		}
	}

	void ClickObjects(Vector2 position, int eventIndex, List<RaycastResult> onRaycastObjects)
	{
		for (int i = 0; i < onRaycastObjects.Count; i++)
		{
			for (int j = 0; j < controlUIobjects.Length; j++)
			{
				if (controlUIobjects[j] != null && onRaycastObjects[i].gameObject == controlUIobjects[j].gameObject)
				{
					controlUIobjects[j].PointerDown(position);
					clickedObjectList.Add(new ClickedObject(eventIndex, controlUIobjects[j]));
				}
			}
		}
	}

	public void Cancel()
	{
		for (int i = 0; i < clickedObjectList.Count; i++)
		{
			clickedObjectList[i].targetObject.Cancel();
		}
		clickedObjectList.Clear();
	}
}
