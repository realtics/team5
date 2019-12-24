using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
	public Button btn;

	public InputField Text;

	public GameObject SaveUI;
	public GameObject LoadUI;

	// Start is called before the first frame update
	void Start()
	{
		btn = this.transform.GetComponent<Button>();
		btn.onClick.AddListener(SendText);
		SaveUI = GameObject.Find("Map").GetComponent<MapGenerator>().saveUI;
		LoadUI = GameObject.Find("Map").GetComponent<MapGenerator>().loadUI;
	}

	// Update is called once per frame
	void Update()
	{

	}


	void SendText()
	{
		if(SaveUI.activeSelf == true)
			Text = GameObject.Find("Map").GetComponent<MapGenerator>().inputSaveText;

		if (LoadUI.activeSelf == true)
			Text = GameObject.Find("Map").GetComponent<MapGenerator>().inputLoadText;

		Text.text = this.transform.Find("Text").GetComponent<Text>().text;
	}
}
