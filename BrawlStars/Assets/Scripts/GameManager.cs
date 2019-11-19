﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    public GameObject mainCanvas;
    public GameObject worldCanvas;
    public Character player;
    public MapSpawner mapSpawner;

    public Joystick moveJoystick;
    public Joystick skillJoystick;

    public Portal[] portals;
    public Character[] monsters;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
