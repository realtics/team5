using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOnOff : MonoBehaviour
{
    public GameObject RoomCanvas;
    public GameObject LobbyCanvas;

    // Start is called before the first frame update
    void Start()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
    }
 
    //로비 창 닫고 룸 열기
    public void ClickOnRoom()
    {
        RoomCanvas.SetActive(true);
        LobbyCanvas.SetActive(false);
    }
    //룸 창 닫고 로비 열기
    public void ClickOnRobby()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
    }
}
