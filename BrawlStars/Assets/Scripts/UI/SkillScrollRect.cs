using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillScrollRect : MonoBehaviour
{
    public RectTransform panel;
    public Button[] bttn;
    public RectTransform center;
    public int startButton = 1;

    public float[] distance;
    public float[] distReposition;
    private bool dragging = false;
    private int bttnDistance;
    private int minButtnNum;
    private int bttnLenght;
    private float lerpSpeed = 5f;

    private void Start()
    {
        bttnLenght = bttn.Length;
        distance = new float[bttnLenght];
        distReposition = new float[bttnLenght];

        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.y - bttn[0].GetComponent<RectTransform>().anchoredPosition.y);
    }

    private void Update()
    {
        for (int i = 0; i < bttn.Length; i++)
        {
            distReposition[i] = center.GetComponent<RectTransform>().position.y - bttn[i].GetComponent<RectTransform>().position.y;
            distance[i] = Mathf.Abs(distReposition[i]);

            if (distReposition[i] > 120)
            {
                float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX , curY+ (bttnLenght * bttnDistance));
                bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
                bttn[i].gameObject.SetActive(false);
            }
            else if (distReposition[i] < -120)
            {
                float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX , curY - (bttnLenght * bttnDistance));
                bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
                bttn[i].gameObject.SetActive(false);
            }
            else
                bttn[i].gameObject.SetActive(true);

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
            //LerpToBttn(minButtnNum * bttnDistance);
            LerpToBttn(-bttn[minButtnNum].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    void LerpToBttn(float position)
    {
        float newY = Mathf.Lerp(panel.anchoredPosition.y, position, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(position - newY) < 3f)
            newY = position;

        Vector2 newPosition = new Vector2(panel.anchoredPosition.x, newY);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        lerpSpeed = 5;
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }
}
