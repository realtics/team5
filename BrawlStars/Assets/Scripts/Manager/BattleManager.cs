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

	public List<GameObject> droppedItemList;

    private void Awake()
    {
        instance = this;
		droppedItemList = new List<GameObject>();
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

	public void DeActivateInputHandler()
	{
		InputHandler inputHandler = mainCanvas.GetComponent<InputHandler>();
		inputHandler.Cancel();
		inputHandler.enabled = false;
	}

	public bool IsAnyItemOnMap()
	{
		return droppedItemList.Count > 0;
	}
}
