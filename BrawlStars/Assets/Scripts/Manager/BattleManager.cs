﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager instance = null;

    public GameObject mainCanvas;
    public GameObject worldCanvas;
    public Character player;
    public MapSpawner mapSpawner;
	public HPBar upperHPBar;
	public InGameLog logView;

	public Joystick moveJoystick;
    public Joystick skillJoystick;
    public SkillIcon[] skillIcons;
	public QuickSlot[] quickSlots;

	List<GameObject> droppedItemList;
	List<Actor> actorListOnMap;

	public DroppedItem droppedItem;
	public GameObject rangeObject;
	public GameObject monsterRange;

    private void Awake()
    {
        instance = this;
		droppedItemList = new List<GameObject>();
		actorListOnMap = new List<Actor>();
	}

	// Start is called before the first frame update
	void Start()
    {
        player = Instantiate(GameManager.GetInstance().player, transform.position, Quaternion.identity);

        moveJoystick.player = player;
        skillJoystick.player = player;
		for(int i = 0; i < skillIcons.Length; i++)
			skillIcons[i].Init(player);
		for (int i = 0; i < quickSlots.Length; i++)
			quickSlots[i].Init(player, GameManager.GetInstance().GetQuickSlotItem());
		mapSpawner.Init(player);
    }

    public static BattleManager GetInstance()
    {
        return instance;
    }

	public void DropItem(string itemCode, Vector3 position)
	{
		Vector3 itemPosition = Vector3.zero;
		itemPosition.x = position.x + Random.Range(-1f, 1f);
		itemPosition.z = position.z + Random.Range(-1f, 1f);

		DroppedItem itemObject = ObjectPool.GetInstance().GetObject(droppedItem.gameObject).GetComponent<DroppedItem>();
		itemObject.itemCode = itemCode;
		droppedItemList.Add(itemObject.gameObject);
		itemObject.transform.position = itemPosition;
	}

	public void PickUpItem(DroppedItem itemForPickUp)
	{
		bool canAddNewItem = GameManager.GetInstance().AddNewItemInInventory(itemForPickUp.itemCode);
		if (canAddNewItem)
		{
			Item item = GameManager.GetInstance().GetItem(itemForPickUp.itemCode);
			logView.AddItemGetLog(item.itemName);
			droppedItemList.Remove(itemForPickUp.gameObject);
			ObjectPool.GetInstance().PushObject(itemForPickUp.gameObject);
		}
	}

	public void ClearAllItem()
	{
		for(int i = 0; i < droppedItemList.Count; i++)
		{
			ObjectPool.GetInstance().PushObject(droppedItemList[i]);
		}
		droppedItemList.Clear();
	}

	public void SetActiveInputHandler(bool value)
	{
		InputHandler inputHandler = mainCanvas.GetComponent<InputHandler>();
		if (value)
		{
			inputHandler.enabled = true;
		}
		else
		{
			inputHandler.Cancel();
			inputHandler.enabled = false;
		}
	}

	public bool IsAnyItemOnMap()
	{
		return droppedItemList.Count > 0;
	}

	public void AddActorOnManager(Actor actor)
	{
		actorListOnMap.Add(actor);
	}

	public void AddActorsOnManager(Actor[] actors)
	{
		actorListOnMap.AddRange(actors);
	}

	public void DeleteActorFromManager(Actor actor)
	{
		actorListOnMap.Remove(actor);
	}

	public void ClearActorList()
	{
		actorListOnMap.Clear();
	}

	public List<Actor> FindActorsInRectangle(Vector3 center, Vector3 halfExtents, Quaternion rotation)
	{
		List<Actor> result = new List<Actor>();
		
		for(int i = 0; i < actorListOnMap.Count; i++)
		{
			Vector3 v1 = rotation * new Vector3(1, 0, 0);
			Vector3 v2 = actorListOnMap[i].transform.position - center;
			v1.y = v2.y = 0;

			float cosValue = Vector3.Dot(v1, v2) / (v1.magnitude * v2.magnitude);
			float sinValue = Mathf.Sqrt(1 - cosValue * cosValue);

			float halfReach = halfExtents.x;
			float halfWidth = halfExtents.z + actorListOnMap[i].GetCollisionRadius();
			if (Mathf.Abs(v2.magnitude * cosValue) < halfReach && Mathf.Abs(v2.magnitude * sinValue) < halfWidth)
				result.Add(actorListOnMap[i]);
		}

		return result;
	}

	public List<Actor> FindActorsInCircle(Vector3 center, float radius)
	{
		List<Actor> result = new List<Actor>();

		for (int i = 0; i < actorListOnMap.Count; i++)
		{
			Vector3 targetDirection = actorListOnMap[i].transform.position - center;
			targetDirection.y = 0;
			float sqrDistance = Mathf.Abs(targetDirection.sqrMagnitude);
			float sqrRadius = Mathf.Pow(radius + actorListOnMap[i].GetCollisionRadius(), 2);
			if (sqrDistance < sqrRadius)
				result.Add(actorListOnMap[i]);
		}

		return result;
	}
	public bool IsEnemyInCircle(Vector3 center, float radius, Team team)
	{
		for (int i = 0; i < actorListOnMap.Count; i++)
		{
			Vector3 targetDirection = actorListOnMap[i].transform.position - center;
			targetDirection.y = 0;
			float sqrDistance = Mathf.Abs(targetDirection.sqrMagnitude);
			float sqrRadius = Mathf.Pow(radius + actorListOnMap[i].GetCollisionRadius(), 2);
			if (sqrDistance < sqrRadius && actorListOnMap[i].team != team)
				return true;
		}
		return false;
	}

	public List<Actor> FindActorsInFanwise(Vector3 center, float radius, float angle, float yRotationEuler)
	{
		List<Actor> result = new List<Actor>();

		for (int i = 0; i < actorListOnMap.Count; i++)
		{
			Vector3 targetDirection = actorListOnMap[i].transform.position - center;
			targetDirection.y = 0;
			float sqrDistance = Mathf.Abs(targetDirection.sqrMagnitude);
			if (sqrDistance < Mathf.Pow(radius + actorListOnMap[i].GetCollisionRadius(), 2))
			{
				float currentAngle = Mathf.Atan2(-targetDirection.z, targetDirection.x);
				float diff = currentAngle - yRotationEuler * Mathf.Deg2Rad;

				while (diff < -Mathf.PI)
					diff += Mathf.PI * 2;
				while (diff > Mathf.PI)
					diff -= Mathf.PI * 2;

				if(Mathf.Abs(diff) < angle / 2)
					result.Add(actorListOnMap[i]);
			}
		}

		return result;
	}
}
