using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvoryShock : Skill
{
    public float reach;
    public float angle;
    const int detail = 20;

    public override void Action()
    {
        transform.rotation = Quaternion.Euler(90, rotationDegree - 90, 0);
        StartCoroutine(DamageCoroutine());
    }

    public override IEnumerator SpriteCoroutine()
    {
        yield return new WaitForSeconds(spriteInterval);
    }

    IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(startupTime);

        Vector3 point = new Vector3(transform.position.x, 0, transform.position.z);
        for (int i = 0; i < damageCount; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(point, reach);
            for (int j = 0; j < colliders.Length; j++)
            {
                if (!IsInFanwise(colliders[j].transform.position))
                    continue;

                Character target = colliders[j].GetComponent<Character>();
                if (target != null && target.team == Team.Enemy)
                {
                    target.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }
        Destroy(gameObject);
    }

    bool IsInFanwise(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        float currentAngle = Mathf.Atan2(-targetDirection.z, targetDirection.x);
        float diff = currentAngle - rotationDegree * Mathf.Deg2Rad;

        while (diff < -Mathf.PI) diff += Mathf.PI * 2;
        while (diff > Mathf.PI) diff -= Mathf.PI * 2;

        return Mathf.Abs(diff) < angle / 2;
    }

    public override Mesh GetMesh()
    {
        Vector3[] vertices = new Vector3[detail + 2];
        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 0; i < detail + 1; i++) {
            float MeshAngle = angle * (detail / 2 - i) / detail;
            vertices[i + 1] = new Vector3(reach * Mathf.Cos(MeshAngle), 0, reach * Mathf.Sin(MeshAngle));
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
        for (int i = 0; i < detail + 1; i++)
        {
            uvs[i + 1] = new Vector2(1.0f / detail * i, 1);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    public override Vector3 GetPosition(Vector2 stickMove, float maxMoveLength)
    {
        return new Vector3(0, 0, 0);
    }

    public override Quaternion GetRotation(Vector2 stickMove)
    {
        float angleRad = Mathf.Atan2(-stickMove.y, stickMove.x);
        return Quaternion.Euler(0, angleRad * Mathf.Rad2Deg, 0);
    }
}
