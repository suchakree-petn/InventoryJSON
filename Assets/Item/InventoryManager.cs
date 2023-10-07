using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    private string GetFilePath()
    {
        return Path.Combine(Application.dataPath, "items.json");
    }

    // Adjusted SaveItemsToJson
    public void SaveItemsToJson()
    {
        List<SerializableItem> serializableItems = new List<SerializableItem>();

        foreach (Item item in itemDatabase.items)
        {
            SerializableItem sItem = new SerializableItem
            {
                _name = item._name,
                _description = item._description,
                _icon = item._icon.ToString(),
                _itemType = item._itemType,
                _itemRarity = item._itemRarity,
                _maxStackCount = item._maxStackCount,
                stackCount = item.stackCount,
                _price = item._price,
                //itemPrefabPath = item.itemPrefab ? item.itemPrefab.name : ""
            };

            serializableItems.Add(sItem);
        }
        /*
        string json = JsonUtility.ToJson(new ItemContainer(serializableItems), true);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log("Items saved to JSON.");
        */

        string json = JsonConvert.SerializeObject(new ItemContainer(serializableItems), Formatting.Indented); // Modified this line
        File.WriteAllText(GetFilePath(), json);
        Debug.Log("Items saved to JSON.");
    }


    [System.Serializable]
    public class ItemContainer
    {
        public List<SerializableItem> list;

        public ItemContainer(List<SerializableItem> list)
        {
            this.list = list;
        }
    }



    





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Save item all");
            SaveItemsToJson();
        }

       
    }
}

[System.Serializable]
public class SerializableItemList
{
    public List<Item> list;

    public SerializableItemList(List<Item> list)
    {
        this.list = list;
    }
}
[System.Serializable]
public class SerializableItem
{
    public string _name;
    public string _description;
    public string _icon; // Use a string to store the path or name of the icon.
    public ItemType _itemType;
    public ItemRarity _itemRarity;
    public int _maxStackCount;
    public int stackCount;
    public int _price;
    //public string itemPrefabPath; // Use a string to store the path or name of the item prefab.
}
