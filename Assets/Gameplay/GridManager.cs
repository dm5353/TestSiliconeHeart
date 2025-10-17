using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private HashSet<Vector2Int> _occupied = new HashSet<Vector2Int>();

    public bool IsAreaFree(Vector2Int origin, int w, int h)
    {
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                if (_occupied.Contains(origin + new Vector2Int(x, y))) return false;
        return true;
    }

    public void OccupyArea(Vector2Int origin, int w, int h)
    {
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                _occupied.Add(origin + new Vector2Int(x, y));
    }

    public void FreeArea(Vector2Int origin, int w, int h)
    {
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                _occupied.Remove(origin + new Vector2Int(x, y));
    }

    public Vector2Int WorldToGrid(Vector3 pos) =>
        new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
}
