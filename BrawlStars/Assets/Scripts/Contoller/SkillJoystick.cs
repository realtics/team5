using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillJoystick : Joystick
{
    public int skillIndex;
    MeshFilter rangeObject;

    Skill skill;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    public override void PointerDown(Vector2 position)
    {
        base.PointerDown(position);
        skill = GameManager.GetInstance().GetSkill(player.skillCodeArray[skillIndex]);
		rangeObject = ObjectPool.GetInstance().GetObject(BattleManager.GetInstance().rangeObject.gameObject).GetComponent<MeshFilter>();
        rangeObject.mesh = skill.GetTargetRangeMesh();
        rangeObject.transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
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
		ObjectPool.GetInstance().AddNewObject(rangeObject.gameObject);

        player.AttackProcess(skillIndex, rangeObject.transform.position, rangeObject.transform.rotation.eulerAngles.y);
    }

	public override void Cancel()
	{
		base.Cancel();
		ObjectPool.GetInstance().AddNewObject(rangeObject.gameObject);
	}
}
