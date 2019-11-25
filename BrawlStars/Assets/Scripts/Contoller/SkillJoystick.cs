using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillJoystick : Joystick
{
    public int skillIndex;
    public SkillRange rangeObject;

    Skill skill;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        skill = player.skillArray[skillIndex];
        rangeObject.SetMesh(skill.GetTargetRangeMesh());
        rangeObject.transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
        rangeObject.DrawRange();
    }

    public override void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);

        rangeObject.transform.position = skill.GetPosition(stickMove, mTransform.sizeDelta.x) + new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
        rangeObject.transform.rotation = skill.GetRotation(stickMove);

        rangeObject.DrawRange();
    }

    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data);
        rangeObject.StopDrawing();

        player.AttackProcess(skillIndex, rangeObject.transform.position, rangeObject.transform.rotation.eulerAngles.y);
    }
}
