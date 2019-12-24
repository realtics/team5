using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Skill
{
    public float radius;
    const int detail = 20;
	public bool traceOwner;

    public override void Action(float yRotationEuler)
    {
        StartCoroutine(DamageCoroutine());
    }

	private void Update()
	{
		if (traceOwner)
			transform.position = owner.transform.position;
	}

	IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(startupTime);

        for (int i = 0; i < damageCount; i++)
        {
			List<Actor> targets = BattleManager.GetInstance().FindActorsInCircle(transform.position, radius);
            for (int j = 0; j < targets.Count; j++)
                if (targets[j] != null && targets[j].team != owner.team)
                    targets[j].TakeDamage(damage);

            yield return new WaitForSeconds(damageInterval);
        }
        Destroy(gameObject);
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

    public override Vector3 GetPosition(Vector2 stickMove, float maxMoveLength)
    {
        return new Vector3(0, 0, 0);
    }

    public override Quaternion GetRotation(Vector2 stickMove)
    {
        Quaternion returnValue = Quaternion.Euler(0, 0, 0);
        return returnValue;
    }
}
