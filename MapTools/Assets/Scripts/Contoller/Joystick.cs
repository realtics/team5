using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : ControlUI
{
    protected RectTransform mTransform;
    Vector2 originalPosition;

    public RectTransform stick;
    protected Vector2 stickMove;
        
    public Character player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public override void PointerDown(Vector2 position)
    {
        mTransform = GetComponent<RectTransform>();
        originalPosition = mTransform.position;
        mTransform.position = position;
        stick.anchoredPosition = new Vector2(0, 0);
    }

    public override void Drag(Vector2 position)
    {
        stickMove = position - new Vector2(mTransform.position.x, mTransform.position.y);
        if (Vector2.SqrMagnitude(stickMove) > Mathf.Pow(mTransform.sizeDelta.x / 2, 2))
        {
            stickMove = stickMove / stickMove.magnitude * mTransform.sizeDelta.x / 2;
        }
        stick.anchoredPosition = stickMove;
    }

    public override void PointerUp(Vector2 position)
    {
        mTransform.position = originalPosition;
        stick.anchoredPosition = new Vector2(0, 0);
    }

	public override void Cancel()
	{
		mTransform.position = originalPosition;
		stick.anchoredPosition = new Vector2(0, 0);
	}
}
