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


	public ScrollRect fileScroll;

	//int tempInt = 2;

	// Start is called before the first frame update
	void Start()
	{
		//CreateFileSlotsInWindow();
	}

	public void CreateFileSlotsInWindow()
	{
		xPos = startingPosX;
		yPos = startingPosY;

		string folderName = @"Assets\Resources\StageMaps\";

		DirectoryInfo directoryInfo = new DirectoryInfo(folderName);

		//foreach (FileInfo file in directoryInfo.GetFiles(tempInt + "*.txt"))

		foreach (FileInfo file in directoryInfo.GetFiles("*.txt"))
		{
			string fileNameOnly = file.Name.Substring(0, file.Name.Length - 4);
			//string FullName = file.FullName;
		
			ButtonSlot = Instantiate(buttonPrefab);
            ButtonSlot.name = fileNameOnly;
			ButtonSlot.transform.Find("Text").GetComponent<Text>().text = fileNameOnly;

			ButtonSlot.transform.SetParent(content.transform);
			ButtonSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
			yPos += (int)ButtonSlot.GetComponent<RectTransform>().rect.height;
		}

		fileScroll.verticalNormalizedPosition = 1;
	}

	public void DeleteFileSlotsInWindow()
	{
		GameObject[] buttonObject = GameObject.FindGameObjectsWithTag("Button");

		for (int i = 0; i < buttonObject.Length; i++)
		{
			Destroy(buttonObject[i]);
		}
	}
}
