using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Actor
{
	public string monsterName;

    public float sight;
    public float attackReach;

    NavMeshAgent pathFinder;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        lastSkillActionTime = new float[skillCodeArray.Length];
        for (int i = 0; i < skillCodeArray.Length; i++)
        {
            lastSkillActionTime[i] = Time.time;
        }

        pathFinder = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (state == State.Idle)
        {
            ChasePlayerCharacter();
        }

    }

    protected override void Death()
    {
        base.Death();
		DropItem();
    }

    void ChasePlayerCharacter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight);

        Character target = null;
        float minSqrDistance = sight * sight;
        Vector3 moveVector;
        for (int i = 0; i < colliders.Length; i++)
        {
            moveVector = colliders[i].transform.position - transform.position;
            if(moveVector.sqrMagnitude < minSqrDistance)
            {
                Character collider = colliders[i].GetComponent<Character>();
                if (collider != null && collider.team == Team.Player)
                {
					minSqrDistance = moveVector.sqrMagnitude;
                    target = collider;
                }
            }
        }

		if (target == null)
		{
			Stop();
			return;
		}

        moveVector = target.transform.position - transform.position;

        if (minSqrDistance > Mathf.Pow(mCollider.radius + attackReach, 2) && state == State.Idle)
        {
            NavMeshPath path = new NavMeshPath();
            pathFinder.CalculatePath(target.transform.position, path);
            
            if (path.corners.Length > 1)
            {
                Move(path.corners[1] - transform.position);
            }
        }
        else
        {
            characterDirectionAngle = Mathf.Atan2(-moveVector.z, moveVector.x);
            Stop();
			ActivatePattern();
		}
    }

    void ActivatePattern()
    {
        for(int i = 0; i < skillCodeArray.Length; i++)
        {
			Skill pattern = GameManager.GetInstance().GetSkill(skillCodeArray[i]);
            if (Time.time - lastSkillActionTime[i] > pattern.cooldown)
            {
                lastSkillActionTime[i] = Time.time;
                Vector3 activatePosition = transform.position + pattern.GetPosition(new Vector2(0, 0), 1);
                float rotationAngle = characterDirectionAngle * Mathf.Rad2Deg;
                AttackProcess(i, activatePosition, rotationAngle);
            }
        }
    }

    void DropItem()
    {
		DropItem[] dropItemList = GameManager.GetInstance().GetDropItemList(monsterName);

        for (int i = 0; i < dropItemList.Length; i++)
			if (Random.Range(0, 100) < dropItemList[i].percentage)
				BattleManager.GetInstance().DropItem(dropItemList[i].itemCode, transform.position);
    }
}
