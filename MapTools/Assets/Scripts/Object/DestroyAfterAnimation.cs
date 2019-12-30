using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
	public Animator animator;

	void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			ObjectPool.GetInstance().AddNewObject(gameObject);
    }
}
