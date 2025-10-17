using UnityEngine;

public class BuildingFactory
{
    public Building Create(string prefabPath, Vector2Int gridPos, string id, int w, int h)
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at: {prefabPath}");
            return null;
        }

        var go = Object.Instantiate(prefab);
        var b = go.GetComponent<Building>() ?? go.AddComponent<Building>();
        b.Init(id, w, h, gridPos);
        return b;
    }
}
