using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public float reach;
    public float width;
    public GameObject fireballObject;
    List<GameObject> fireballList = new List<GameObject>();

	public override void Action(float yRotationEuler)
	{
		damage = attackPercentage * status.attackDamage / 100;

		Quaternion rotation = Quaternion.Euler(0f, yRotationEuler, 0f);
		Vector3 normalVector = rotation * new Vector3(1, 0, 0);

		for (int i = 1; i <= reach; i++)
		{
			GameObject effect = ObjectPool.GetInstance().GetObject(fireballObject);
			effect.transform.position = transform.position + normalVector * i;
			fireballList.Add(effect);
		}

		StartCoroutine(DamageCoroutine(yRotationEuler));
	}

    IEnumerator DamageCoroutine(float yRotationEuler)
    {
        yield return new WaitForSeconds(startupTime);
        
        Vector3 targetPosition = new Vector3(reach / 2 * Mathf.Cos(-yRotationEuler * Mathf.Deg2Rad), 0, reach / 2 * Mathf.Sin(-yRotationEuler * Mathf.Deg2Rad));
        Vector3 center = new Vector3(transform.position.x + targetPosition.x, 0, transform.position.z + targetPosition.z);
        for (int i = 0; i < damageCount; i++)
        {
            Collider[] colliders = Physics.OverlapBox(center, new Vector3(reach / 2, 0, width / 2), Quaternion.Euler(0, yRotationEuler, 0));
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

        for (int i = 0; i < fireballList.Count; i++)
		{
			ObjectPool.GetInstance().AddNewObject(fireballList[i]);
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
