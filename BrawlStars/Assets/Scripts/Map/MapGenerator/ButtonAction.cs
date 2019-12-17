using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
	public Button btn;

	public InputField load;

	// Start is called before the first frame update
	void Start()
	{
		btn = this.transform.GetComponent<Button>();
		btn.onClick.AddListener(SendText);
		load = GameObject.Find("Map").GetComponent<MapGenerator>().inputLoadText;
	}

	// Update is called once per frame
	void Update()
	{

	}


	void SendText()
	{
		load.text = this.transform.Find("Text").GetComponent<Text>().text;
	}
}
