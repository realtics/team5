using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScrollRect : MonoBehaviour
{
	public ScrollRect scrollRect;
	bool mouseDown, buttonDown, buttonUp;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (mouseDown)
		{
			if (buttonDown)
			{
				ScrollDown();
			}
			else if (buttonUp)
			{
				ScrollUp();
			}
		}
	}

	public void ButtonDownisPressed()
	{
		mouseDown = true;
		buttonDown = true;
		
	}

	public void ButtonUpisPressed()
	{
		mouseDown = true;
		buttonUp = true;
	}

	private void ScrollDown()
	{
		if (Input.GetMouseButtonUp(0))
		{
			mouseDown = false;
			buttonDown = false;
		}
		else 
		{
			scrollRect.verticalNormalizedPosition -= 0.01f;
		}
	}

	private void ScrollUp()
	{
		if (Input.GetMouseButtonUp(0))
		{
			mouseDown = false;
			buttonUp = false;
		}
		else
		{
			scrollRect.verticalNormalizedPosition += 0.01f;
		}
	}
}
