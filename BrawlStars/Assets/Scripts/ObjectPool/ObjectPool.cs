using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	public string objectName;
	List<GameObject> objectList = new List<GameObject>();

	public ObjectPool(string name)
	{
		objectName = name;
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
