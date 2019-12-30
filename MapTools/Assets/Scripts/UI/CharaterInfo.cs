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

        text.text = GameManager.GetInstance().GetPlayerInfoString();
    }
}
