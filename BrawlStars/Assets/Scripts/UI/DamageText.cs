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

    public void SetDefaultPosition(Vector3 position, int damage)
    {
        originalPosition = position;
		transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Text>().text = "-" + damage.ToString();

        instantiatedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - instantiatedTime > destroyTime)
            Destroy(gameObject);

        originalPosition = new Vector3(originalPosition.x, originalPosition.y + moveSpeed * Time.deltaTime, originalPosition.z);
        transform.position = Camera.main.WorldToScreenPoint(originalPosition);
    }
}
