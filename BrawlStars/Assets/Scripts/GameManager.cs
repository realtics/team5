using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    public Character player;
    public ItemSlot[] equippedSlot;
    public Item[] equippedItem;
    public Status itemStatus;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;

        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].SetSlotIndex(i);
        }
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        equippedItem = new Item[equippedSlot.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetEquippedItem(int index, Item item)
    {
        equippedItem[index] = item;

        itemStatus.attackDamage = 0;
        itemStatus.armor = 0;
        itemStatus.hp = 0;
        itemStatus.hpRecovery = 0;
        itemStatus.moveSpeed = 0;

        for (int i = 0; i < equippedItem.Length; i++)
        {
            if (equippedItem[i] != null)
            {
                itemStatus.attackDamage += equippedItem[i].status.attackDamage;
                itemStatus.armor += equippedItem[i].status.armor;
                itemStatus.hp += equippedItem[i].status.hp;
                itemStatus.hpRecovery += equippedItem[i].status.hpRecovery;
                itemStatus.moveSpeed += equippedItem[i].status.moveSpeed;
            }
        }
    }
}