using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    [SerializeField] private List<UIInventory> allInventoryType = new();
    public List<SlotItem> itemList = new List<SlotItem>();
    public int _maxSlot;

    [Header("MoneyMain")]
    public int _money;
    public TextMeshProUGUI ShowMoneyMain;
    void Awake()
    {
        Instance = this;
        MoneyInGameWhenStart();
    }
    
    public void MoneyInGameWhenStart()
    {
        _money = 800;
        ShowMoneyMain.text = _money + "";
    }

    
    public void BuyItem()
    {
        int priceItem = UIInventory.GetpriceNow;
        if (_money >= priceItem)
        {
            _money -= priceItem;
            ShowMoneyMain.text = _money + "";
            Debug.Log("Buy this item");
        }
        else
        {
            Debug.Log("Can not buy this item");
        }
    }

    public void Add(Item item, ref int amount)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].item._name == item._name && itemList[i].stackCount + amount <= itemList[i].maxStackCount)
            {
                itemList[i].stackCount += amount;
                amount = 0;
                return;
            }
            else if (itemList[i].item._name == item._name && itemList[i].stackCount + amount > itemList[i].maxStackCount)
            {
                int deficit = itemList[i].stackCount + amount - item._maxStackCount;
                itemList[i].stackCount += amount - deficit;
                amount = deficit;
                return;
            }
        }

        if (itemList.Count < _maxSlot)
        {
            itemList.Add(new SlotItem(item, amount, item._maxStackCount));
            amount = 0;
        }
    }
    public void Remove(Item item, int amount)
    {
        foreach (SlotItem slot in itemList)
        {
            if (slot.item == item)
            {
                if (slot.maxStackCount > amount)
                {
                    slot.stackCount -= amount;
                }
                else
                {
                    itemList.Remove(slot);
                    return;
                }
            }
        }
    }
}
[System.Serializable]
public class SlotItem
{
    public Item item;
    public int stackCount;
    public int maxStackCount;
    public SlotItem(Item item, int amount, int maxStackCount)
    {
        this.item = item;
        this.stackCount = amount;
        this.maxStackCount = maxStackCount;
    }
}

