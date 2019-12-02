using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystick : Joystick
{
    public override void Drag(PointerEventData data)
    {
        base.Drag(data);
        Vector3 playerMove = new Vector3(stickMove.x, 0, stickMove.y);
        player.Move(playerMove);
    }

    public override void PointerUp(PointerEventData data)
    {
        base.PointerUp(data);
        player.Stop();
    }
}
