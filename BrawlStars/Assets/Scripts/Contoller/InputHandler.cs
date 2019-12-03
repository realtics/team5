using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{ 
	GraphicRaycaster raycast;
	public ControlUI[] controlUIobjects;

	PointerEventData eventData;
	List<RaycastResult> raycastResults;
	List<ControlUI> clickedObjectList;
	bool isOnClick;

	Touch touchInfo;

	// Use this for initialization
	void Start()
	{
		raycast = GetComponent<GraphicRaycaster>();
		eventData = new PointerEventData(null);
		raycastResults = new List<RaycastResult>();
		clickedObjectList = new List<ControlUI>();

		isOnClick = false;
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		touchInfo = Input.GetTouch(0);
#endif

		HandlePointerDown();
		HandlePointerUp();
		HandleDrag();
	}

	void HandlePointerDown()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			eventData.position = Input.mousePosition;
#elif UNITY_ANDROID
		if(touchInfo.phase == TouchPhase.Began) {
			eventData.position = touchInfo.position;
#endif
			raycastResults.Clear();
			raycast.Raycast(eventData, raycastResults);

			for (int i = 0; i < raycastResults.Count; i++)
				for (int j = 0; j < controlUIobjects.Length; j++)
					if (controlUIobjects[j] != null && raycastResults[i].gameObject == controlUIobjects[j].gameObject)
					{
						controlUIobjects[j].PointerDown(eventData);
						clickedObjectList.Add(controlUIobjects[j]);
					}

			isOnClick = true;
		}
	}

	void HandleDrag()
	{
		if (isOnClick)
		{
#if UNITY_EDITOR
			eventData.position = Input.mousePosition;
#elif UNITY_ANDROID
			eventData.position = touchInfo.position;
#endif
			for (int i = 0; i < clickedObjectList.Count; i++)
				clickedObjectList[i].Drag(eventData);
		}
	}

	void HandlePointerUp()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButtonUp(0))
		{
			eventData.position = Input.mousePosition;
#elif UNITY_ANDROID
		if(touchInfo.phase == TouchPhase.Ended) {
			eventData.position = touchInfo.position;
#endif
			for (int i = 0; i < clickedObjectList.Count; i++)
				clickedObjectList[i].PointerUp(eventData);

			clickedObjectList.Clear();
			isOnClick = false;
		}
	}
}
