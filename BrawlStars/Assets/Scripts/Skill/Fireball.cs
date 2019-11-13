using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public float reach;
    public float width;
    public GameObject fireballObject;
    public List<GameObject> fireballList;
    
    public override void Action()
    {
        Quaternion rotation = Quaternion.Euler(0f, rotationDegree, 0f);
        Vector3 normalVector = rotation * new Vector3(1, 0, 0);

        for(int i = 1; i <= reach; i++)
        {
            GameObject effect = Instantiate(fireballObject, transform.position + normalVector * i, Quaternion.identity);
            fireballList.Add(effect);
        }

        StartCoroutine(DamageCoroutine());
    }
    
    public override IEnumerator SpriteCoroutine()
    {
        yield return new WaitForSeconds(spriteInterval);
    }

    IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(startupTime);
        
        Vector3 targetPosition = new Vector3(reach / 2 * Mathf.Cos(-rotationDegree * Mathf.Deg2Rad), 0, reach / 2 * Mathf.Sin(-rotationDegree * Mathf.Deg2Rad));
        Vector3 center = new Vector3(transform.position.x + targetPosition.x, 0, transform.position.z + targetPosition.z);
        for (int i = 0; i < damageCount; i++)
        {
            Collider[] colliders = Physics.OverlapBox(center, new Vector3(reach / 2, 0, width / 2), Quaternion.Euler(0, rotationDegree, 0));
            for (int j = 0; j < colliders.Length; j++)
            {
                Character target = colliders[j].GetComponent<Character>();
                if (target != null && target.team == Team.Enemy)
                {
                    target.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }

        for (int i = 0; i < fireballList.Count; i++)
        {
            Destroy(fireballList[i]);
        }
        Destroy(gameObject);
    }

    public override Mesh GetMesh()
    {
        Vector3[] vertices = {
            new Vector3(0, 0, -width / 2), new Vector3(0, 0, width / 2),
            new Vector3(reach, 0, -width / 2), new Vector3(reach, 0, width / 2)
        };
        int[] triangles = { 0, 1, 2, 2, 1, 3 };
        Vector2[] uvs = { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1) };

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
        float degree = Mathf.Atan2(-stickMove.y, stickMove.x);
        return Quaternion.Euler(0, degree * Mathf.Rad2Deg, 0);
    }
}
