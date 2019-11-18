using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float castingDelay;
    public float startupTime;
    public float recoveryTime;

    public int damage;
    public int damageCount;
    public float damageInterval;

    public float cooldown;
    protected float lastUsedTime;

    public float spriteInterval;

    protected Character owner;

    public void StartSkill(Character user, Vector3 position, float yRotationEuler)
    {
        StartCooldown();
        Skill actionSkill = Instantiate(this, position, Quaternion.identity);
        actionSkill.owner = user;
        actionSkill.Action(yRotationEuler);
    }

    public abstract void Action(float yRotationEuler);
    public abstract Mesh GetTargetRange();
    public abstract Vector3 GetPosition(Vector2 stickMove, float maxMoveLength);
    public abstract Quaternion GetRotation(Vector2 stickMove);
    
    public void InitCooldown()
    {
        lastUsedTime = Time.time - cooldown;
    }

    public void StartCooldown()
    {
        lastUsedTime = Time.time;
    }

    public float GetRemainCooldown()
    {
        return cooldown - (Time.time - lastUsedTime);
    }

    public bool ReadyToAction()
    {
        return GetRemainCooldown() < 0;
    }
}
