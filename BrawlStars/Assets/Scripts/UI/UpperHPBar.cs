using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpperHPBar : MonoBehaviour
{
	public Character player;
	
	public Image image;
	public Text HpText;
	

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {

		printHp();
	}

	void printHp()
	{
		//image.fillAmount = player.hpBar.hp / player.hpBar.maxHp;
		//HpText.text = player.hpBar.hp.ToString();

	}
}
