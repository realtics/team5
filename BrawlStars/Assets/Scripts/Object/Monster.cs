using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float sight;
    public Skill patternArray;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        ChasePlayerCharacter();
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
                if (collider.team == Team.Player)
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
}
