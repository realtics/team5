using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameLog : MonoBehaviour
{
	Text logText;
	float limitTime = 0;
	List<string> logList;
	public int maxLogCount;
	public string getItemString;

    // Start is called before the first frame update
    void Start()
    {
		logText = GetComponent<Text>();
		logList = new List<string>();

		gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		if (logList.Count == 0)
		{
			gameObject.SetActive(false);
		} else
		{
			if (limitTime >= 3.0f)
			{
				limitTime = 0f;
				logList.RemoveAt(0);
				PrintLog();
			}
		}

		limitTime += Time.deltaTime;
    }

	public void Init()
	{
		logList.Clear();
	}

	public void AddItemGetLog(string ItemName)
	{
		gameObject.SetActive(true);
		logList.Add(getItemString.Replace("[ITEM]", ItemName));
		if(logList.Count > maxLogCount)
			logList.RemoveAt(0);
		limitTime = 0f;
		PrintLog();
	}

	void PrintLog()
	{
		logText.text = "";
		for(int i = 0; i < logList.Count; i++)
		{
			logText.text += logList[i] + "\n";
		}
	}
}
