using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    WEAPON, ARMOR, ACCESSORY, SUB, ETC
}

public class Item : MonoBehaviour
{
    public Sprite icon;
    public Type type;

    public Stat stat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
