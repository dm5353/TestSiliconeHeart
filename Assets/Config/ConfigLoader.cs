using System.IO;
using UnityEngine;

public static class ConfigLoader
{
    public static BuildingsConfig Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "BuildingsConfig.json");
        if (!File.Exists(path))
        {
            Debug.LogError("BuildingsConfig.json not found: " + path);
            return new BuildingsConfig();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<BuildingsConfig>(json);
    }
}
