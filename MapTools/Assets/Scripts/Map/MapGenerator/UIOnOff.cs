using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnOff : MonoBehaviour
{

	public GameObject onGameObject;

    // Start is called before the first frame update
    void Start()
    {
		if(onGameObject.activeSelf != false)
			onGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void panelOn()
	{
		onGameObject.SetActive(true);
	}

	public void panelOff()
	{
		onGameObject.SetActive(false);
	}
}
