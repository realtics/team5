using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public string objectName;
	List<GameObject> objectList = new List<GameObject>();

	public void Init(GameObject gameObject, int count)
	{
		objectName = gameObject.name;
		for (int i = 0; i < count; i++)
		{
			GameObject firstObject = Instantiate(gameObject);
			firstObject.name = gameObject.name;
			firstObject.SetActive(false);
			AddNewObject(firstObject);
		}
	}

	public void AddNewObject(GameObject newObject)
	{
		objectList.Add(newObject);
	}

	public GameObject GetObject()
	{
		if (objectList.Count > 0)
		{
			GameObject returnValue = objectList[0];
			objectList.RemoveAt(0);
			return returnValue;
		}
		else
			return null;
	}
}
