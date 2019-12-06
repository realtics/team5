using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnSpawner : MonoBehaviour
{
	public Text countText;
	public Button minusBtn;
	public Button plusBtn;

	public Text numberText;
	public Button numberButton;

    // Start is called before the first frame update
    void Start()
    {
		countText.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
