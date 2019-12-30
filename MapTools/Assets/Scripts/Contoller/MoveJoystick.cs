using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystick : Joystick
{
    public override void Drag(Vector2 position)
    {
        base.Drag(position);
        Vector3 playerMove = new Vector3(stickMove.x, 0, stickMove.y);
        player.Move(playerMove);
    }

    public override void PointerUp(Vector2 position)
    {
        base.PointerUp(position);
        player.Stop();
    }

	public override void Cancel()
	{
		base.Cancel();
		player.Stop();
	}
}
