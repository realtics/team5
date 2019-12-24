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

	public CharaterInfo infoWindow;

	public ItemSlot[] equippedSlot;

    public GameObject stageSelecter;
	public StageSelector[] stageArray;

	public Image imageForDrag;

    private void Awake()
    {
        instance = this;

		GameManager.GetInstance().InitEquipSlot(equippedSlot.Length);
		infoWindow.SetCharacterInfo();
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
		stageSelecter.SetActive(false);
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
		stageSelecter.SetActive(true);
		stageArray[0].Select();
	}

    public void SelectCloseBtn()
    {
		stageSelecter.SetActive(false);
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

	public void DragSkillImage(Sprite image, Vector2 position)
	{
		imageForDrag.gameObject.SetActive(true);
		imageForDrag.sprite = image;
		imageForDrag.transform.position = position;
	}

	public void DragFinish()
	{
		imageForDrag.gameObject.SetActive(false);
	}
}
