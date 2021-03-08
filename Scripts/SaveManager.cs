using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;
    public Save save;

    public static SaveManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SaveManager found!");
            return;
        }
        instance = this;
        savePath = string.Join("/", Application.persistentDataPath, "save.json");
        Load();
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            LogsManager.instance.WriteLog($"Found save at {savePath}, loading it...");
            string json = File.ReadAllText(savePath);
            save = JsonConvert.DeserializeObject<Save>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            LogsManager.instance.WriteLog($"Loaded previous save!");
        }
        else
        {
            try
            {
                save = new Save();
                Save();
            }
            catch (System.Exception e)
            {
                LogsManager.instance.WriteLog(e.ToString());
            }
            LogsManager.instance.WriteLog($"Created new save at {savePath}!");
        }
        foreach (Block block in save.blocks)
        {
            MapGenerator.instance.CreateBlockUI(block).transform.SetParent(GameObject.Find("Blocks").transform);
        }
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(save, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        File.WriteAllText(savePath, json);
    }
}

[Serializable]
public class Save
{
    public float money = 10f;
    public List<Block> blocks = new List<Block>();
    public Statistics stats = new Statistics();

    public void AddBlock(Block block)
    {
        blocks.Add(block);
        SaveManager.instance.Save();
    }

    public void ModifyBlock(Block block)
    {
        blocks.Remove(blocks.FirstOrDefault(b => b.id == block.id));
        AddBlock(block);
    }

    public void DeleteBlock(Block block)
    {
        blocks.Remove(block);
        SaveManager.instance.Save();
    }
}

[Serializable]
public class Statistics
{
    public List<CraftItem> sold = new List<CraftItem>();
    public List<CraftItem> crafted = new List<CraftItem>();

    public void AddItemToSold(Item item)
    {
        foreach (CraftItem craftItem in sold)
        {
            if (craftItem.itemId == item.itemId)
            {
                craftItem.number++;
                return;
            }
        }
        sold.Add(new CraftItem(1, item.itemId));
    }

    public void AddItemToCrafted(Item item)
    {
        foreach (CraftItem craftItem in crafted)
        {
            if (craftItem.itemId == item.itemId)
            {
                craftItem.number++;
                return;
            }
        }
        crafted.Add(new CraftItem(1, item.itemId));
    }
}