﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : Skill
{
    public float reach;
    public float radius;

    const int detail = 60;
    Vector3 position;

    public override void Action(float yRotationEuler)
    {
		damage = attackPercentage * status.attackDamage / 100;
        StartCoroutine(DamageCoroutine());
    }

    IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(startupTime);

        Vector3 point = new Vector3(transform.position.x, 0, transform.position.z);
        for (int i =0; i < damageCount; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(point, radius);
            for (int j = 0; j < colliders.Length; j++)
            {
                Actor target = colliders[j].GetComponent<Actor>();
                if (target != null && target.team != owner.team)
                {
                    target.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }

		ObjectPool.GetInstance().AddNewObject(gameObject);
    }

    public override void MakeTargetRangeMesh()
    {
        Vector3[] vertices = new Vector3[detail + 2];
        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 1; i <= detail + 1; i++)
        {
            vertices[i] = new Vector3(radius * Mathf.Cos(Mathf.PI * 2 * (detail / 2 - i) / detail), 0, radius * Mathf.Sin(Mathf.PI * 2 * (detail / 2 - i) / detail));
        }

        int[] triangles = new int[detail * 3];
        for (int i = 0; i < detail; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        Vector2[] uvs = new Vector2[detail + 2];
        uvs[0] = new Vector2(0.5f, 0);
        for (int i = 1; i <= detail + 1; i++)
        {
            uvs[i] = new Vector2(1.0f / detail * (i - 1) / 2 + 0.5f, 1);
        }

        rangeMesh = new Mesh();
        rangeMesh.vertices = vertices;
        rangeMesh.triangles = triangles;
        rangeMesh.uv = uvs;
    }

    public override Vector3 GetPosition(Vector2 stickMove, float maxStickMoveLength)
    {
        position = new Vector3(reach * stickMove.x / maxStickMoveLength, 0.1f, reach * stickMove.y / maxStickMoveLength);
        return position;
    }

    public override Quaternion GetRotation(Vector2 stickMove)
    {
        float angleRad = Mathf.Atan2(-stickMove.y, stickMove.x);
        return Quaternion.Euler(0, angleRad * Mathf.Rad2Deg, 0);
    }
}
