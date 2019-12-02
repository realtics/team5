using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameLog : MonoBehaviour
{
	public ScrollRect scrollRect;

	public Text logText = null;

	float limitTime = 0;

    // Start is called before the first frame update
    void Start()
    {
		scrollRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		//if (Input.GetMouseButtonDown(0))
		//{
		//	scrollRect.gameObject.SetActive(true);

		//	logText.text += " " + "Mouse Down" + "\n";

		//	scrollRect.verticalNormalizedPosition = 0.0f;
		//}

		if (limitTime >= 3.0f)
		{
			scrollRect.gameObject.SetActive(false);
			limitTime = 0f;
		}
		else
		{
			limitTime += Time.deltaTime;
		}

    }

	public void GameLog(string ItemName)
	{
		scrollRect.gameObject.SetActive(true);

		logText.text += " " + ItemName + "\n";

		scrollRect.verticalNormalizedPosition = 0.0f;
	}
}
