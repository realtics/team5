using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillJoystick : Joystick
{
    public Skill skill;
    public SkillRange rangeObject;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
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

        skill.StartSkill(player, rangeObject.transform.position, rangeObject.transform.rotation.eulerAngles.y);
        player.AttackProcess(1, rangeObject.transform.rotation.eulerAngles.y);
    }
}
