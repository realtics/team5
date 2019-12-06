using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager
{
	static ObjectPoolManager instance;
	List<ObjectPool> pooledObjectList;

	public static ObjectPoolManager GetInstance()
	{
		if (instance == null)
			instance = new ObjectPoolManager();
		return instance;
	}

	ObjectPoolManager()
    {
		pooledObjectList = new List<ObjectPool>();
    }

	public void AddNewObject(string name, GameObject newObject)
	{
		ObjectPool poolForAdd = GetPool(name);
		if (poolForAdd == null)
		{
			poolForAdd = new ObjectPool(name);
			pooledObjectList.Add(poolForAdd);
		}
		poolForAdd.AddNewObject(newObject);
	}

	public GameObject GetObject(string name)
	{
		ObjectPool poolForAdd = GetPool(name);
		if (poolForAdd != null)
			return poolForAdd.GetObject();
		else
			return null;
	}

	public ObjectPool GetPool(string name)
	{
		for (int i = 0; i < pooledObjectList.Count; i++)
		{
			if (pooledObjectList[i].objectName == name)
				return pooledObjectList[i];
		}
		return null;
	}
}
