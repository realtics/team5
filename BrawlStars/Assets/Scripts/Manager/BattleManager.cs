using System.Collections;
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
    public SkillIcon[] skillIcon;

	List<GameObject> droppedItemList;
	List<Actor> actorListOnMap;

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
		for(int i = 0; i < skillIcon.Length; i++)
			skillIcon[i].Init(player);
		mapSpawner.Init(player);
    }

    public static BattleManager GetInstance()
    {
        return instance;
    }

	public void DropItem(string itemName, Vector3 position)
	{
		Vector3 itemPosition = Vector3.zero;
		itemPosition.x = position.x + Random.Range(-1f, 1f);
		itemPosition.z = position.z + Random.Range(-1f, 1f);

		Item dropItem = GameManager.GetInstance().GetItem(itemName);
		GameObject itemObject = ObjectPool.GetInstance().GetObject(dropItem.gameObject);
		droppedItemList.Add(itemObject);
		itemObject.transform.position = itemPosition;
	}

	public void PickUpItem(Item item)
	{
		bool canAddNewItem = GameManager.GetInstance().AddNewItemInInventory(item.itemCode);
		if (canAddNewItem)
		{
			logView.AddItemGetLog(item.itemName);
			droppedItemList.Remove(item.gameObject);
			ObjectPool.GetInstance().AddNewObject(item.gameObject);
		}
	}

	public void ClearAllItem()
	{
		for(int i = 0; i < droppedItemList.Count; i++)
		{
			ObjectPool.GetInstance().AddNewObject(droppedItemList[i]);
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
			if (sqrDistance < radius * radius)
				result.Add(actorListOnMap[i]);
		}

		return result;
	}

	public List<Actor> FindActorsInFanwise(Vector3 center, float radius, float angle, float yRotationEuler)
	{
		List<Actor> result = new List<Actor>();

		for (int i = 0; i < actorListOnMap.Count; i++)
		{
			Vector3 targetDirection = actorListOnMap[i].transform.position - center;
			targetDirection.y = 0;
			float sqrDistance = Mathf.Abs(targetDirection.sqrMagnitude);
			if (sqrDistance < radius * radius)
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
