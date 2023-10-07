using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Weapon,
    Consumable,
    Relic
}
public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
public class Item : ScriptableObject
{
    [Header("Info")]
    public string _name;
    [TextArea(4,6)]
    public string _description;
    public string _icon;
    public ItemType _itemType;
    public ItemRarity _itemRarity;
    public int _maxStackCount = 10;
    public int stackCount = 1;
    public int _price;
    

    //[Header("Prefab")]
    //public GameObject itemPrefab;

    void Awake()
    {
        _itemType = ItemType.Default;
    }
}
