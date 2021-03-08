using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
 
public class CSVExporter : EditorWindow
{
    public string logs;

    [MenuItem("Window/CSV Exporter")]
    public static void ShowWindow()
    { 
        GetWindow<CSVExporter>("CSV Exporter");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Start CSV exportation"))
        {
            Export(); 
        }
    }

    void Export()
    {
        DateTime before = DateTime.Now;
        string str = "";
        string filePath = "C:/Users/Maxou/Desktop/CSVExporter/csv.csv";
        if (File.Exists(filePath))
            File.Delete(filePath);
        foreach (ItemUI itemUI in Resources.LoadAll<ItemUI>("UIs/Items"))
            str += new ItemToExport(itemUI).ToString() + "\n";
        File.WriteAllText(filePath, str);
        Debug.Log($"CSV Exportation finished in {((TimeSpan)(DateTime.Now - before)).TotalMilliseconds}ms!");
    }
}

class ItemToExport
{
    public string itemId;
    public string itemUrl;
    public string isInput;
    public float cost;
    public string craftMachine;
    public string craftInputs;
    public string craftOutputs;

    public ItemToExport(ItemUI itemUI)
    {
        this.itemId = itemUI.name;
        this.itemUrl = itemUI.sprite.name;
        this.isInput = itemUI.isInput ? "Yes" : "No";
        this.cost = itemUI.price;
        FindCraft();
    }

    public override string ToString()
    {
        return string.Join(";", itemId, itemUrl, isInput, cost, craftMachine, craftInputs, craftOutputs);
    }

    public void FindCraft()
    {
        foreach (string dir in Directory.GetDirectories("E:\\Documents\\Unity\\SpaceAutomation\\Assets\\Resources\\UIs\\Crafts"))
        {
            string[] tmp = dir.Split('\\');
            string machine = tmp[tmp.Length - 1];
            foreach (CraftUI craftUI in Resources.LoadAll<CraftUI>("UIs\\Crafts\\" + machine))
            {
                bool found = false;
                foreach (CraftItem item in craftUI.outputs)
                {
                    if (item.itemId == this.itemId)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    craftMachine = machine;
                    List<string> inputs = new List<string>();
                    List<string> outputs = new List<string>();
                    foreach (CraftItem item in craftUI.inputs)
                    {
                        inputs.Add($"{item.number} {item.itemId}");
                    }
                    foreach (CraftItem item in craftUI.outputs)
                    {
                        outputs.Add($"{item.number} {item.itemId}");
                    }
                    craftInputs = string.Join("-", inputs);
                    craftOutputs = string.Join("-", outputs);
                }
            }
        }
    }
}
