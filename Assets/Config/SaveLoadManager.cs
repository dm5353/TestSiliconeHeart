using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlacedBuildingDTO
{
    public string id;
    public int x, y, w, h;
}

[System.Serializable]
public class PlacedBuildingsData
{
    public List<PlacedBuildingDTO> items = new List<PlacedBuildingDTO>();
}

public static class SaveLoadManager
{
    static string FilePath => Path.Combine(Application.persistentDataPath, "placed_buildings.json");

    public static void Save(PlacedBuildingsData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
        Debug.Log("Saved: " + FilePath);
    }

    public static PlacedBuildingsData Load()
    {
        if (!File.Exists(FilePath))
            return new PlacedBuildingsData();

        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<PlacedBuildingsData>(json);
    }
}
