﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager instance = null;

    public GameObject mainCanvas;
    public GameObject worldCanvas;
    public Character player;
    public MapSpawner mapSpawner;
	public HPBar upperHPBar;
	public InGameLog logView;

	public Joystick moveJoystick;
    public Joystick skillJoystick;
    public SkillIcon[] skillIcon;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(GameManager.GetInstance().player, transform.position, Quaternion.identity);

        moveJoystick.player = player;
        skillJoystick.player = player;
		for(int i = 0; i < skillIcon.Length; i++)
			skillIcon[i].Init(player);
		mapSpawner.Init(player);
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