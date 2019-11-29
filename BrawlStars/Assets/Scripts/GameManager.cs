using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    public Character player;
    Dictionary<string, Item> itemTable;
    public Item[] itemTableElements;
    public string[] InventoryItemNameArray;
    public string[] equippedItemNameArray;
    Status itemStatus;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemTable = new Dictionary<string, Item>();
        for(int i = 0; i < itemTableElements.Length; i++)
        {
            itemTable.Add(itemTableElements[i].itemCode, itemTableElements[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitInventory(int row, int column)
    {
        if (InventoryItemNameArray.Length == 0)
        {
            InventoryItemNameArray = new string[row * column];
            for (int i = 0; i < InventoryItemNameArray.Length; i++)
            {
                InventoryItemNameArray[i] = null;
            }
        }
    }

    public bool AddNewItemInInventory(Item item)
    {
        int emptySlotIndex;
        for (emptySlotIndex = 0; emptySlotIndex < InventoryItemNameArray.Length; emptySlotIndex++)
        {
            if (InventoryItemNameArray[emptySlotIndex] == null)
                break;
        }

        if(emptySlotIndex < InventoryItemNameArray.Length)
        {
            InventoryItemNameArray[emptySlotIndex] = item.itemCode;
            return true;
        } else
        {
            return false;
        }
    }

    public Item GetItemInInventory(int index)
    {
        if (InventoryItemNameArray[index] == null)
            return null;
        else
            return itemTable[InventoryItemNameArray[index]];
    }

    public Item GetEquippedItem(int index)
    {
        if (equippedItemNameArray[index] == null)
            return null;
        else
            return itemTable[equippedItemNameArray[index]];
    }

    public void SwapSlot(int index1, int index2)
    {
        string temp = InventoryItemNameArray[index1];
        InventoryItemNameArray[index1] = InventoryItemNameArray[index2];
        InventoryItemNameArray[index2] = temp;
    }

    public void EquipItem(int equippedIndex, int originalIndex)
    {
        string temp = InventoryItemNameArray[originalIndex];
        InventoryItemNameArray[originalIndex] = equippedItemNameArray[equippedIndex];
        equippedItemNameArray[equippedIndex] = temp;

        itemStatus.attackDamage = 0;
        itemStatus.armor = 0;
        itemStatus.hp = 0;
        itemStatus.hpRecovery = 0;
        itemStatus.moveSpeed = 0;

        for (int i = 0; i < equippedItemNameArray.Length; i++)
        {
            if (equippedItemNameArray[i] != null)
            {
                itemStatus += itemTable[equippedItemNameArray[i]].status;
            }
        }
    }

    public Status GetFinalStatus()
    {
        return player.status + itemStatus;
    }

    public string GetPlayerInfoString()
    {
        Status finalStatus = player.status + itemStatus;
        string content = "공격력 : " + finalStatus.attackDamage + " (" + player.status.attackDamage + " + " + itemStatus.attackDamage + ")\n";
        content += "방어력 : " + finalStatus.armor + " (" + player.status.armor + " + " + itemStatus.armor + ")\n";
        content += "체력 : " + finalStatus.hp + " (" + player.status.hp + " + " + itemStatus.hp + ")\n";
        content += "체력회복 : " + finalStatus.hpRecovery + " (" + player.status.hpRecovery + " + " + itemStatus.hpRecovery + ")\n";
        content += "이동속도 : " + finalStatus.moveSpeed + " (" + player.status.moveSpeed + " + " + itemStatus.moveSpeed + ")\n";
        return content;
    }
}