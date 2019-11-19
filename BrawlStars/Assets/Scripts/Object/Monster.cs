using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Character
{
    public float sight;
    public Skill[] patternArray;
    float[] lastSkillActionTime;
    public float attackReach;

    NavMeshAgent pathFinder;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        lastSkillActionTime = new float[patternArray.Length];
        for (int i = 0; i < patternArray.Length; i++)
        {
            lastSkillActionTime[i] = Time.time;
            //patternArray[i].StartCooldown();
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
            ActivatePattern();
        }
    }

    void ChasePlayerCharacter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight);

        Character target = null;
        float minDistance = sight * sight;
        Vector3 moveVector;
        for (int i = 0; i < colliders.Length; i++)
        {
            moveVector = colliders[i].transform.position - transform.position;
            if(moveVector.sqrMagnitude < minDistance)
            {
                Character collider = colliders[i].GetComponent<Character>();
                if (collider != null && collider.team == Team.Player)
                {
                    minDistance = moveVector.sqrMagnitude;
                    target = collider;
                }
            }
        }

        if (target == null)
            return;

        moveVector = target.transform.position - transform.position;

        if (minDistance > Mathf.Pow(mCollider.radius + attackReach, 2) && state == State.Idle)
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
    }

    void ActivatePattern()
    {
        for(int i = 0; i < patternArray.Length; i++)
        {
            if (Time.time - lastSkillActionTime[i] > patternArray[i].cooldown)
            {
                Vector3 position = patternArray[i].GetPosition(new Vector2(0, 0), 1);
                Quaternion rotation = patternArray[i].GetRotation(new Vector2(0, 0));
                patternArray[i].StartSkill(this, transform.position + position, rotation.eulerAngles.y);
                lastSkillActionTime[i] = Time.time;

                AttackProcess(0, characterDirectionAngle * Mathf.Rad2Deg);
            }
        }
    }
}
