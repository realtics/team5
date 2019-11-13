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
        rangeObject.SetMesh(skill.GetMesh());
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

        skill.StartCooldown();
        Skill actionSkill = Instantiate(skill, rangeObject.transform.position, Quaternion.identity);
        player.AttackProcess(1, rangeObject.transform.rotation.eulerAngles.y);
        actionSkill.SetAngleDegree(rangeObject.transform.rotation.eulerAngles.y);
        actionSkill.Action();
    }
}
