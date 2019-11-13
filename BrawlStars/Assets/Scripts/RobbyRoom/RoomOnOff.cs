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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnRoom()
    {
        RoomCanvas.SetActive(true);
        LobbyCanvas.SetActive(false);
    }

    public void ClickOnRobby()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
    }
}
