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
	Animator animator;
	List<Actor> damagedTargetList;

    public override void Action(float yRotationEuler)
    {
        direction = Quaternion.Euler(0, yRotationEuler, 0) * new Vector3(1, 0, 0);
		animator = GetComponent<Animator>();
		animator.SetInteger("direction", GetDirectionIndex(yRotationEuler * Mathf.Deg2Rad));
		damagedTargetList = new List<Actor>();
		StartCoroutine(DamageCoroutine());
	}

    IEnumerator DamageCoroutine()
    {
        float duration = reach / speed;
        float actionTime = Time.time;
        
        while(Time.time - actionTime < duration)
		{
			List<Actor> targets = BattleManager.GetInstance().FindActorsInCircle(transform.position, width);
			for (int j = 0; j < targets.Count; j++)
				if (!damagedTargetList.Contains(targets[j]) && targets[j].team != owner.team)
				{
					targets[j].TakeDamage(damage, attackPercentage);
					damagedTargetList.Add(targets[j]);
				}

			transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
			yield return null;
        }

        ObjectPool.GetInstance().PushObject(gameObject);
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

	int GetDirectionIndex(float radianValue)
	{
		float angleBasedZAxis = Global.ConvertIn2PI(radianValue + Mathf.PI / 2, -Mathf.PI);

		if (angleBasedZAxis > 0)
			transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		else
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

		int totalDirectionCount = (spriteDirectionCount - 1) * 2;
		int result = 0;
		for(int i = totalDirectionCount - 1; i > 0; i -= 2)
		{
			if (angleBasedZAxis < -Mathf.PI * i / totalDirectionCount || angleBasedZAxis > Mathf.PI * i / totalDirectionCount)
				return result;
			result++;
		}
		return spriteDirectionCount - 1;
	}

	public override bool IsTargetInRange(Actor target, Vector3 origin)
	{
		Vector3 targetVector = target.transform.position - origin;
		return targetVector.sqrMagnitude < Mathf.Pow(reach, 2);
	}
}
