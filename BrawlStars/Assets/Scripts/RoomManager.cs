using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Menu
{
    public string name;
    public GameObject content;
}

public class RoomManager : MonoBehaviour
{
    static RoomManager instance = null;

    public Menu[] menuArray;
    int menuIndex;
    public Text menuText;

    public ItemSlot[] equippedSlot;

    public GameObject StageSelecter;

    private void Awake()
    {
        instance = this;
    }

    public RoomManager Getinstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        menuIndex = 0;
        SetMenuActive(0);
        StageSelecter.SetActive(false);

        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].SetSlotIndex(i);
        }

        GameManager.GetInstance().equippedItemNameArray = new string[equippedSlot.Length];
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

}
