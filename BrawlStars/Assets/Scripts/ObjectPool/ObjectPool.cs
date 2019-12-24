using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct preInstantiatedPrefab
{
	public GameObject gameObject;
	public int count;
}

public class ObjectPool : MonoBehaviour
{
	static ObjectPool instance;
	List<PooledObject> poolList;
	public preInstantiatedPrefab[] prefabList;

	private void Awake()
	{
		instance = this;

		poolList = new List<PooledObject>();
		for(int i = 0; i < prefabList.Length; i++)
		{
			poolList.Add(gameObject.AddComponent<PooledObject>());
			poolList[i].Init(prefabList[i].gameObject, prefabList[i].count);
		}
	}

	public static ObjectPool GetInstance()
	{
		return instance;
	}

	public void AddNewObject(GameObject newObject)
	{
		PooledObject poolForAdd = GetPool(newObject.name);
		if (poolForAdd == null)
		{
			poolForAdd = gameObject.AddComponent<PooledObject>();
			poolForAdd.Init(newObject, 10);
			poolList.Add(poolForAdd);
		}
		newObject.SetActive(false);
		poolForAdd.AddNewObject(newObject);
	}

	public GameObject GetObject(GameObject gameObject)
	{
		PooledObject poolForAdd = GetPool(gameObject.name);

		GameObject result;
		if (poolForAdd != null)
			result = poolForAdd.GetObject();
		else
			result = Instantiate(gameObject);

		result.SetActive(true);
		result.name = gameObject.name;
		return result;
	}

	public PooledObject GetPool(string name)
	{
		for (int i = 0; i < poolList.Count; i++)
		{
			if (poolList[i].objectName == name)
				return poolList[i];
		}
		return null;
	}
}
