using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    float instantiatedTime;

    public void SetDefaultPosition(Vector3 position, int damage)
    {
        transform.localPosition = Camera.main.WorldToScreenPoint(position);
        GetComponent<Text>().text = "-" + damage.ToString();

        instantiatedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - instantiatedTime > destroyTime)
            Destroy(gameObject);

        transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);
    }
}
