using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRect_Snap : MonoBehaviour
{
    public RectTransform panel;
    public Button[] bttn;
    public RectTransform center;

    public float[] distance;
    private bool dragging = false;
    private int bttnDistance;
    private int minButtnNum;
    private int bttnLenght;

    private void Start()
    {
        bttnLenght = bttn.Length;
        distance = new float[bttnLenght];

        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
    }

    private void Update()
    {
        for (int i = 0; i < bttn.Length; i++)
        {
            distance[i] = Mathf.Abs(center.GetComponent<RectTransform>().position.x - bttn[i].GetComponent<RectTransform>().position.x);
        }

        float minDistance = Mathf.Min(distance);

        for (int j = 0; j < bttn.Length; j++)
        {
            if (minDistance == distance[j])
            {
                minButtnNum = j;
            }
        }

        if (!dragging)
        {
            LerpToBttn(minButtnNum * -bttnDistance);
        }
    }

    void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10.0f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void SetActive()
    {
        this.gameObject.SetActive(true);
    }

}
