﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
	public string skillName;
    public Sprite icon;

    public float castingDelay;
    public float startupTime;
    public float recoveryTime;

    public int attackPercentage;
    protected int damage;
    public int damageCount;
    public float damageInterval;

    public float cooldown;
    protected float lastUsedTime;

    public float spriteInterval;

    protected Actor owner;
    protected Mesh rangeMesh;

    protected Animator animator;
    protected Status status;

    public void StartSkill(Actor user, Vector3 position, float yRotationEuler)
    {
		Skill actionSkill = ObjectPool.GetInstance().GetObject(gameObject).GetComponent<Skill>();
		actionSkill.gameObject.SetActive(true);
		actionSkill.transform.position = position;
        actionSkill.StartCooldown();
        actionSkill.owner = user;
        actionSkill.status = user.GetFinalStatus();
        actionSkill.Action(yRotationEuler);
    }

    public abstract void Action(float yRotationEuler);
    public abstract void MakeTargetRangeMesh();
    public abstract Vector3 GetPosition(Vector2 stickMove, float maxMoveLength);
    public abstract Quaternion GetRotation(Vector2 stickMove);
    
    public Mesh GetTargetRangeMesh()
    {
        return rangeMesh;
    }

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
