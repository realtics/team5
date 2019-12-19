using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class FileWindow : MonoBehaviour
{
	public int startingPosX;
	public int startingPosY;
	public int slotCntPerPage;
	public int slotCntLength;
	public GameObject buttonPrefab;
	public GameObject content;

	private int xPos;
	private int yPos;
	private GameObject ButtonSlot;
	private int ButtonSlotCnt = 0;

	public ScrollRect fileScroll;

	// Start is called before the first frame update
	void Start()
	{
		
		CreateInventorySlotsInWindow();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void CreateInventorySlotsInWindow()
	{
		xPos = startingPosX;
		yPos = startingPosY;

		string folderName = @"Assets\StageMaps\";

		DirectoryInfo directoryInfo = new DirectoryInfo(folderName);

		foreach (FileInfo file in directoryInfo.GetFiles("*.txt"))
		{
			string FileNameOnly = file.Name.Substring(0, file.Name.Length - 4);
			string FullName = file.FullName;
		
			ButtonSlot = Instantiate(buttonPrefab);
			ButtonSlot.transform.Find("Text").GetComponent<Text>().text = FileNameOnly;

			ButtonSlot.transform.SetParent(content.transform);
			ButtonSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
			xPos += (int)ButtonSlot.GetComponent<RectTransform>().rect.width;

			ButtonSlotCnt++;

			if (ButtonSlotCnt % slotCntLength == 0)
			{
				ButtonSlotCnt = 0;
				yPos -= (int)ButtonSlot.GetComponent<RectTransform>().rect.height;
				xPos = startingPosX;
			}
		}

		fileScroll.verticalNormalizedPosition = 1;
	}
}
