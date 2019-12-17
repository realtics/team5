using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomManager : MonoBehaviour
{
    static RoomManager instance = null;

    public Menu[] menuArray;
    int menuIndex;
    public Text menuText;

    public ItemSlot[] equippedSlot;

    public GameObject StageSelecter;
	public StageSelector[] stageArray;

    private void Awake()
    {
        instance = this;

		GameManager.GetInstance().InitEquipSlot(equippedSlot.Length);
	}

    public static RoomManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
	{
		menuIndex = 0;
        SetMenuActive(0);
        StageSelecter.SetActive(false);

		stageArray[0].Select();
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenNextMenu()
    {
        menuIndex = (menuIndex + 1) % menuArray.Length;
        //SetMenuActive();
    }

    public void OpenPrevMenu()
    {
        menuIndex = (menuIndex - 1 + menuArray.Length) % menuArray.Length;
        //SetMenuActive();
    }

    public void SetMenuActive(int menuIndex)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            if (i == menuIndex)
            {
                menuArray[i].content.SetActive(true);
            }
            else
                menuArray[i].content.SetActive(false);
        }
    }

    public void SelectStartBtn()
    {
        StageSelecter.SetActive(true);
    }

    public void SelectCloseBtn()
    {
        StageSelecter.SetActive(false);
    }

	public void SelectStage(int index)
	{
		GameManager.GetInstance().stageIndex = index;
	}

	public void SizeDownAllStageSelector()
	{
		for(int i = 0; i < stageArray.Length; i++)
		{
			stageArray[i].SizeDown();
		}
	}
}
