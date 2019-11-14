﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float sight;
    public Skill[] patternArray;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < patternArray.Length; i++)
        {
            patternArray[i].StartCooldown();
        }
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
        for(int i = 0; i < colliders.Length; i++)
        {
            Vector3 moveVector = colliders[i].transform.position - transform.position;
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

        if(target != null)
        {
            Move(target.transform.position - transform.position);
        }
        else
        {
            Stop();
        }
    }

    void ActivatePattern()
    {
        for(int i = 0; i < patternArray.Length; i++)
        {
            if (patternArray[i].ReadyToAction())
            {
                Vector3 position = patternArray[i].GetPosition(new Vector2(0, 0), 1);
                Quaternion rotation = patternArray[i].GetRotation(new Vector2(0, 0));
                patternArray[i].StartSkill(this, transform.position + position, rotation.eulerAngles.y);
                AttackProcess(0, transform.rotation.eulerAngles.y);
            }
        }
    }
}
