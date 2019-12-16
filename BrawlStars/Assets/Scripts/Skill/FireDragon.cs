using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragon : Skill
{
    public float reach;
    public float width;
    public float speed;
    public int spriteDirectionCount;

    Vector3 direction;

    public override void Action(float yRotationEuler)
    {
        transform.rotation = Quaternion.Euler(90, yRotationEuler - 90, 0);
        direction = Quaternion.Euler(0, yRotationEuler, 0) * new Vector3(1, 0, 0);
        StartCoroutine(DamageCoroutine());
    }

    IEnumerator DamageCoroutine()
    {
        float duration = reach / speed;
        float actionTime = Time.time;
        
        while(Time.time - actionTime < duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            yield return null;
        }

        ObjectPool.GetInstance().AddNewObject(gameObject);
    }

    public override void MakeTargetRangeMesh()
    {
        Vector3[] vertices = {
            new Vector3(0, 0, -width / 2), new Vector3(0, 0, width / 2),
            new Vector3(reach, 0, -width / 2), new Vector3(reach, 0, width / 2)
        };
        int[] triangles = { 0, 1, 2, 2, 1, 3 };
        Vector2[] uvs = { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) };

        rangeMesh = new Mesh();
        rangeMesh.vertices = vertices;
        rangeMesh.triangles = triangles;
        rangeMesh.uv = uvs;
    }

    public override Vector3 GetPosition(Vector2 stickMove, float maxStickMoveLength)
    {
        return new Vector3(0, 0, 0);
    }

    public override Quaternion GetRotation(Vector2 stickMove)
    {
        float degree = Mathf.Atan2(-stickMove.y, stickMove.x);
        return Quaternion.Euler(0, degree * Mathf.Rad2Deg, 0);
    }
}
