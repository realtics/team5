using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public float reach;
    public float width;
    public GameObject fireballObject;
    GameObject[] fireballList;

	public override void Action(float yRotationEuler)
	{
		transform.rotation = Quaternion.Euler(0f, yRotationEuler, 0f);
		Vector3 normalVector = transform.rotation * new Vector3(1, 0, 0);

		if(fireballList == null)
			fireballList = new GameObject[(int)reach];

		for (int i = 1; i <= fireballList.Length; i++)
		{
			GameObject effect = ObjectPool.GetInstance().GetObject(fireballObject);
			effect.transform.position = transform.position + normalVector * i;
			fireballList[i-1] = effect;
		}

		transform.position += normalVector * reach / 2;

		StartCoroutine(DamageCoroutine(yRotationEuler));
	}

    IEnumerator DamageCoroutine(float yRotationEuler)
    {
        yield return new WaitForSeconds(startupTime);
        
        for (int i = 0; i < damageCount; i++)
        {
			Vector3 halfExtents = new Vector3(reach, 0, width) / 2;
			Quaternion rotation = Quaternion.Euler(0, yRotationEuler, 0);

			List<Actor> targets = BattleManager.GetInstance().FindActorsInRectangle(transform.position, halfExtents, rotation);
            for (int j = 0; j < targets.Count; j++)
                if (targets[j] != null && targets[j].team != owner.team)
                    targets[j].TakeDamage(damage);

            yield return new WaitForSeconds(damageInterval);
        }

        for (int i = 0; i < fireballList.Length; i++)
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

	public override bool IsTargetInRange(Actor target, Vector3 origin)
	{
		Vector3 targetVector = target.transform.position - origin;
		return targetVector.sqrMagnitude < Mathf.Pow(reach, 2);
	}
}
