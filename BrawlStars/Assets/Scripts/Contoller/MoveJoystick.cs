using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystick : Joystick
{
    public override void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);
        Vector3 playerMove = new Vector3(stickMove.x, 0, stickMove.y);
        player.Move(playerMove);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data);
        player.Stop();
    }
}
