using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float startupTime;
    public int damage;
    public int damageCount;
    public float damageInterval;

    public float cooldown;
    protected float lastUsedTime;

    public float spriteInterval;

    protected float rotationDegree;

    public abstract void Action();
    public abstract IEnumerator SpriteCoroutine();
    public abstract Mesh GetMesh();
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
    
    public void SetAngleDegree(float angle)
    {
        rotationDegree = angle;
    }
}
