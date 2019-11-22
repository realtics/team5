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
        SetMenuActive();
        StageSelecter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenNextMenu()
    {
        menuIndex = (menuIndex + 1) % menuArray.Length;
        SetMenuActive();
    }

    public void OpenPrevMenu()
    {
        menuIndex = (menuIndex - 1 + menuArray.Length) % menuArray.Length;
        SetMenuActive();
    }

    void SetMenuActive()
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            if (i == menuIndex)
            {
                menuArray[i].content.SetActive(true);
                menuText.text = menuArray[i].name;
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
