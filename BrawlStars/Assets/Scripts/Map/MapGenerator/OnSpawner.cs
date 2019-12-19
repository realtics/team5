using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnSpawner : MonoBehaviour
{
	public Text countText;

	int count = 1;

	public GameObject MonsterButton;
	public GameObject SpawnerUI;

	public int divide = -1;

    // Start is called before the first frame update
    void Start()
    {
		countText.text = count.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void onPlusButton()
	{
		if (count < 5)
			count++;
		else
			count = 5;

		countText.text = count.ToString();
	}

	public void onMinusButton()
	{
		if (count > 1)
			count--;
		else
			count = 0;

		countText.text = count.ToString();
	}

	public void onOkButton()
	{
		string holderName = "SpawnerUI(Clone)";

		if (this.transform.Find(holderName))
		{
			Destroy(this.transform.Find(holderName).gameObject);
		}

		GameObject mapHolder = Instantiate(SpawnerUI, new Vector3(this.transform.position.x,
			this.transform.position.y + SpawnerUI.GetComponent<RectTransform>().anchoredPosition.y -(count * count), 0), Quaternion.identity);

		mapHolder.transform.localScale = new Vector3(1, (float)count / 2, 1);

		mapHolder.gameObject.transform.SetParent(transform);

		GameObject[] MonsterObjects = new GameObject[count];

		for (int i = 0; i < count; i++)
		{
			divide *= -1;

			MonsterObjects[i] = Instantiate(MonsterButton, new Vector3(this.transform.position.x + mapHolder.gameObject.transform.localPosition.x,
				(this.transform.position.y + SpawnerUI.GetComponent<RectTransform>().anchoredPosition.y + 30) - (i * 35), mapHolder.gameObject.transform.localPosition.z),
				Quaternion.identity);

			MonsterObjects[i].gameObject.transform.SetParent(mapHolder.gameObject.transform);
		}
	}

}
