using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager instance = null;

    public GameObject mainCanvas;
    public GameObject worldCanvas;
    public Character player;
    public MapSpawner mapSpawner;

    public Joystick moveJoystick;
    public Joystick skillJoystick;

    public Portal[] portals;
    public Monster[] monsters;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(player);

        moveJoystick.player = player;
        skillJoystick.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static BattleManager GetInstance()
    {
        return instance;
    }
}
