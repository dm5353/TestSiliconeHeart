#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class ConfigEditorWindow : EditorWindow
{
    private BuildingsConfig config;
    private string path = Path.Combine(Application.streamingAssetsPath, "BuildingsConfig.json");
    private Vector2 scrollPos;

    [MenuItem("Tools/Buildings Config Editor")]
    public static void Open() => GetWindow<ConfigEditorWindow>("Buildings Config");

    private void OnEnable()
    {
        if (File.Exists(path))
            config = JsonUtility.FromJson<BuildingsConfig>(File.ReadAllText(path));
        else
            config = new BuildingsConfig();
    }

    private void OnGUI()
    {
        if (config == null) return;

        EditorGUILayout.LabelField("Buildings Configuration", EditorStyles.boldLabel);
        config.ppu = EditorGUILayout.IntField("PPU", config.ppu);

        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        int indexToRemove = -1; // помечаем, что нужно удалить после отрисовки

        for (int i = 0; i < config.buildings.Count; i++)
        {
            var b = config.buildings[i];

            EditorGUILayout.BeginVertical("box");

            b.id = EditorGUILayout.TextField("ID", b.id);
            b.prefabPath = EditorGUILayout.TextField("Prefab Path", b.prefabPath);
            b.width = EditorGUILayout.IntField("Width", b.width);
            b.height = EditorGUILayout.IntField("Height", b.height);

            if (GUILayout.Button("Удалить здание"))
                indexToRemove = i;

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        if (indexToRemove >= 0)
            config.buildings.RemoveAt(indexToRemove);

        if (GUILayout.Button("Добавить здание"))
        {
            config.buildings.Add(new BuildingEntry()
            {
                id = "new_building",
                prefabPath = "",
                width = 1,
                height = 1
            });
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Сохранить"))
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, JsonUtility.ToJson(config, true));
            AssetDatabase.Refresh();
            Debug.Log($"Config saved to {path}");
        }
    }
}
#endif