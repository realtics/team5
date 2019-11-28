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
        player = Instantiate(GameManager.GetInstance().player, transform.position, Quaternion.identity);
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
