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
        Character target = BattleManager.GetInstance().player;
        float minSqrDistance = sight * sight;

		if (target == null || target.state == State.Dead)
		{
			Stop();
			return;
		}

		Vector3 moveVector = target.transform.position - transform.position;

        if (minSqrDistance > Mathf.Pow(mCollider.radius + attackReach, 2))
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
		}
		ActivatePattern(target);
	}

    void ActivatePattern(Actor target)
    {
        for(int i = 0; i < skillCodeArray.Length; i++)
        {
			Skill pattern = GameManager.GetInstance().GetSkill(skillCodeArray[i]);

			bool readyToActivatePattern = Time.time - lastSkillActionTime[i] > pattern.cooldown;
			bool playerInPatternRadius = pattern.IsTargetInRange(target, transform.position);
			if (readyToActivatePattern && playerInPatternRadius)
            {
                lastSkillActionTime[i] = Time.time;
				Vector3 targetVector = target.transform.position - transform.position;
				Vector3 activatePosition = transform.position + pattern.GetPosition(new Vector2(targetVector.x, targetVector.z));
                Quaternion rotation = pattern.GetRotation(new Vector2(targetVector.x, targetVector.z));
                AttackProcess(i, activatePosition, rotation.eulerAngles.y);
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
