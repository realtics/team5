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
    public SkillIcon[] skillIcon;
    
    public Portal[] portals;
    public Monster[] monsters;
    public Item[] items;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.GetInstance().player;

        player = Instantiate(player, transform.position, Quaternion.identity);

        mapSpawner.player = player;
        moveJoystick.player = player;
        skillJoystick.player = player;

        for(int i = 0; i < skillIcon.Length; i++)
            skillIcon[i].player = player;//얘만 왜 null인가.
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
