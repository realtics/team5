using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBackground;
    public Image hpBar;
    public Text hpText;

    int hp;
    int maxHp;

	private void Start()
	{
		transform.localScale = new Vector3(1, 1, 1);
	}

	public void SetMaxHp(int hp)
    {
        maxHp = hp;
    }

    public void SetHp(int _hp)
    {
        hp = _hp;
        hpText.text = hp.ToString();
        hpBar.rectTransform.sizeDelta = new Vector2(hpBackground.rectTransform.sizeDelta.x * hp / maxHp, hpBackground.rectTransform.sizeDelta.y);
        hpBar.rectTransform.anchoredPosition = new Vector2(hpBar.rectTransform.sizeDelta.x / 2, hpBar.rectTransform.anchoredPosition.y);
    }

    public void IncreaseHp(int hpChange)
    {
        SetHp(hp + hpChange);
    }

    public void SetHpFull()
    {
        SetHp(maxHp);
    }
}
