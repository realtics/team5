using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillJoystick : Joystick
{
    int skillIndex;
    MeshFilter rangeObject;

    Skill skill;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    public void Init(MeshFilter _rangeObject, int _skillIndex)
    {
        rangeObject = _rangeObject;
        skillIndex = _skillIndex;
    }

    public override void PointerDown(Vector2 position)
    {
        base.PointerDown(position);
        skill = GameManager.GetInstance().GetSkill(player.skillCodeArray[skillIndex]);
    }

    public override void Drag(Vector2 position)
    {
        base.Drag(position);

		Vector3 positionXZ = player.transform.position;
		positionXZ.y = 0.1f;
        rangeObject.transform.position = skill.GetPosition(stickMove, mTransform.sizeDelta.x) + positionXZ;
        rangeObject.transform.rotation = skill.GetRotation(stickMove);
    }

    public override void PointerUp(Vector2 position)
    {
        base.PointerUp(position);
    }

	public override void Cancel()
	{
		base.Cancel();
		ObjectPool.GetInstance().PushObject(rangeObject.gameObject);
	}
}
