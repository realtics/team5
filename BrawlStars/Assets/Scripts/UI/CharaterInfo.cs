using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterInfo : MonoBehaviour
{
    Text text;

    private void OnEnable()
    {
        SetCharacterInfo();
    }

    public void SetCharacterInfo()
    {
        if (text == null)
            text = GetComponent<Text>();

        string content = "공격력 : " + GameManager.GetInstance().player.status.attackDamage;
        content += " + " + GameManager.GetInstance().itemStatus.attackDamage + "\n";
        content += "방어력 : " + GameManager.GetInstance().player.status.armor;
        content += " + " + GameManager.GetInstance().itemStatus.armor + "\n";
        content += "체력 : " + GameManager.GetInstance().player.status.hp;
        content += " + " + GameManager.GetInstance().itemStatus.hp + "\n";
        content += "체력회복 : " + GameManager.GetInstance().player.status.hpRecovery;
        content += " + " + GameManager.GetInstance().itemStatus.hpRecovery + "\n";
        content += "이동속도 : " + GameManager.GetInstance().player.status.moveSpeed;
        content += " + " + GameManager.GetInstance().itemStatus.moveSpeed + "\n";
        text.text = content;
    }
}
