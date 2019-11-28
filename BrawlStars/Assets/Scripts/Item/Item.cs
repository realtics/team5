using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    WEAPON, ARMOR, ACCESSORY, SUB, ETC
}

public class Item : MonoBehaviour
{
    public string itemCode;
    public string itemName;
    public Sprite icon;
    public Type type;
    public Status status;
    public string etc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetItemExplanation()
    {
        string result = itemName + "\n\n";
        switch(type)
        {
            case Type.WEAPON:
                result += "무기\n";
                break;
            case Type.ARMOR:
                result += "방어구\n";
                break;
            case Type.ACCESSORY:
                result += "악세사리\n";
                break;
            case Type.SUB:
                result += "보조장비\n";
                break;
            default:
                break;
        }

        if (status.attackDamage > 0)
            result += "공격력 : " + status.attackDamage + "\n";
        if (status.armor > 0)
            result += "방어력 : " + status.armor + "\n";
        if (status.hp > 0)
            result += "체력 : " + status.hp + "\n";
        if (status.hpRecovery > 0)
            result += "체력회복 : " + status.hpRecovery + "\n";
        if (status.moveSpeed > 0)
            result += "이동속도 : " + status.moveSpeed + "\n";

        result += "\n" + etc;

        return result;
    }
}
