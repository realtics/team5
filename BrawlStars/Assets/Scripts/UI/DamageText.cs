using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    Vector3 originalPosition;
    float instantiatedTime;

	public void Init(int damage)
	{
		originalPosition = transform.position;
		transform.localScale = new Vector3(1, 1, 1);
		Text text = GetComponent<Text>();
		if (damage > 0)
		{
			text.text = damage.ToString();
			text.color = Color.red;
		}
		else
		{
			text.text = (-damage).ToString();
			text.color = Color.green;
		}

		instantiatedTime = Time.time;
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.time - instantiatedTime > destroyTime)
            ObjectPool.GetInstance().AddNewObject(gameObject);

        originalPosition = new Vector3(originalPosition.x, originalPosition.y + moveSpeed * Time.deltaTime, originalPosition.z);
        transform.position = Camera.main.WorldToScreenPoint(originalPosition);
    }
}
