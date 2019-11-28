using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    protected RectTransform mTransform;
    Vector2 originalPosition;

    public RectTransform stick;
    protected Vector2 stickMove;
        
    public Character player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = BattleManager.GetInstance().player;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public virtual void OnPointerDown(PointerEventData data)
    {
        mTransform = GetComponent<RectTransform>();
        originalPosition = mTransform.position;
        mTransform.position = data.position;
        stick.anchoredPosition = new Vector2(0, 0);
    }

    public virtual void OnDrag(PointerEventData data)
    {
        stickMove = data.position - new Vector2(mTransform.position.x, mTransform.position.y);
        if (Vector2.SqrMagnitude(stickMove) > Mathf.Pow(mTransform.sizeDelta.x / 2, 2))
        {
            stickMove = stickMove / stickMove.magnitude * mTransform.sizeDelta.x / 2;
        }
        stick.anchoredPosition = stickMove;
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        mTransform.position = originalPosition;
        stick.anchoredPosition = new Vector2(0, 0);
    }
}
